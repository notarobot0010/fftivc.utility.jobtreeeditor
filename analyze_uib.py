"""
UIB File Analyzer for FFT Ivalice Chronicles
=============================================
Analyzes .uib files to identify widget structures, string tables,
position data, and record patterns.

Usage:
    python analyze_uib.py <file.uib>
    python analyze_uib.py <file.uib> > output.txt

No external packages required - uses only Python standard library.
"""

import struct
import sys
import os


# ============================================================
# Helper functions
# ============================================================

def u32(data, offset):
    """Read unsigned 32-bit little-endian integer."""
    return struct.unpack_from('<I', data, offset)[0]

def s32(data, offset):
    """Read signed 32-bit little-endian integer."""
    return struct.unpack_from('<i', data, offset)[0]

def f32(data, offset):
    """Read 32-bit little-endian float."""
    return struct.unpack_from('<f', data, offset)[0]

def read_cstring(data, offset):
    """Read a null-terminated ASCII string."""
    end = data.index(b'\x00', offset)
    return data[offset:end].decode('ascii', errors='replace')

def is_printable_ascii(byte_val):
    """Check if a byte is printable ASCII."""
    return 0x20 <= byte_val <= 0x7E


# ============================================================
# Phase 1: Header analysis
# ============================================================

def analyze_header(data):
    print("=" * 70)
    print("PHASE 1: FILE HEADER")
    print("=" * 70)
    print(f"File size: {len(data)} bytes (0x{len(data):X})")
    print(f"Magic: {data[0:4]} ({data[0:4].decode('ascii', errors='replace')})")
    print()

    print("Non-zero header values (first 256 bytes):")
    for off in range(0, min(256, len(data)), 4):
        v = u32(data, off)
        if v != 0:
            # Check if the value could be a file offset
            is_offset = 0 < v < len(data)
            note = ""
            if is_offset and v > 0x100:
                note = "  <- possible file offset"
            print(f"  0x{off:04X}: {v:10d} (0x{v:08X}){note}")
    print()


# ============================================================
# Phase 2: String table discovery
# ============================================================

def find_string_tables(data):
    print("=" * 70)
    print("PHASE 2: STRING TABLES")
    print("=" * 70)

    # Look for runs of printable ASCII followed by null bytes
    # String tables tend to be near the end of the file
    tables = []
    min_string_len = 4
    i = len(data) // 2  # Start searching from halfway through the file

    in_string = False
    string_start = 0
    run_start = 0
    strings_in_run = []

    while i < len(data):
        if is_printable_ascii(data[i]):
            if not in_string:
                string_start = i
                in_string = True
        elif data[i] == 0x00 and in_string:
            # End of a string
            s = data[string_start:i].decode('ascii', errors='replace')
            if len(s) >= min_string_len:
                if not strings_in_run:
                    run_start = string_start
                strings_in_run.append((string_start, s))
            in_string = False
        else:
            # Non-printable, non-null: end of any string table run
            if len(strings_in_run) >= 3:
                tables.append((run_start, list(strings_in_run)))
            strings_in_run = []
            in_string = False
        i += 1

    # Don't forget the last run
    if len(strings_in_run) >= 3:
        tables.append((run_start, list(strings_in_run)))

    for t_idx, (start, strings) in enumerate(tables):
        print(f"\nString table {t_idx} starting at 0x{start:05X} ({len(strings)} strings):")
        for s_idx, (off, s) in enumerate(strings):
            # Truncate very long strings for readability
            display = s if len(s) <= 80 else s[:77] + "..."
            print(f"  [{s_idx:3d}] 0x{off:05X}: {display}")

    if not tables:
        print("  No string tables found.")

    print()
    return tables


# ============================================================
# Phase 3: Search for string references
# ============================================================

def find_string_references(data, string_tables):
    print("=" * 70)
    print("PHASE 3: STRING REFERENCES")
    print("=" * 70)

    # For each string, search the file for 4-byte LE pointers to its offset
    ref_count = 0
    for t_idx, (_, strings) in enumerate(string_tables):
        print(f"\nReferences to string table {t_idx}:")
        found_any = False
        for s_idx, (str_off, s) in enumerate(strings[:50]):  # Limit to first 50
            target = struct.pack('<I', str_off)
            positions = []
            pos = 0
            while True:
                p = data.find(target, pos)
                if p == -1:
                    break
                positions.append(p)
                pos = p + 1

            if positions:
                found_any = True
                ref_count += len(positions)
                refs = [f"0x{p:05X}" for p in positions[:5]]
                extra = f" (+{len(positions)-5} more)" if len(positions) > 5 else ""
                display = s if len(s) <= 40 else s[:37] + "..."
                print(f"  \"{display}\" (0x{str_off:05X}): refs at {', '.join(refs)}{extra}")

        if not found_any:
            print("  No direct 4-byte pointer references found.")
            print("  (Strings may be referenced by index or relative offset.)")

    print(f"\nTotal references found: {ref_count}")
    print()


# ============================================================
# Phase 4: Known value pattern search
# ============================================================

def search_known_patterns(data):
    print("=" * 70)
    print("PHASE 4: KNOWN VALUE PATTERNS")
    print("=" * 70)

    # --- Screen dimensions ---
    print("\n--- Screen dimension values ---")
    for label, value in [("1920", 1920), ("1080", 1080), ("960 (half-width)", 960), ("540 (half-height)", 540)]:
        target = struct.pack('<I', value)
        hits = []
        pos = 0
        while True:
            p = data.find(target, pos)
            if p == -1:
                break
            hits.append(p)
            pos = p + 1
        if hits:
            locs = [f"0x{h:05X}" for h in hits[:10]]
            extra = f" (+{len(hits)-10} more)" if len(hits) > 10 else ""
            print(f"  {label} ({value}): {len(hits)} hits at {', '.join(locs)}{extra}")

    # --- Opacity/scale pattern: FF 00 00 00 64 00 00 00 64 00 00 00 64 00 00 00 ---
    print("\n--- Opacity/Scale pattern (255, 100, 100, 100) ---")
    pattern = b'\xff\x00\x00\x00\x64\x00\x00\x00\x64\x00\x00\x00\x64\x00\x00\x00'
    hits = []
    pos = 0
    while True:
        p = data.find(pattern, pos)
        if p == -1:
            break
        hits.append(p)
        pos = p + 1
    print(f"  Found {len(hits)} occurrences:")
    for h in hits:
        # Print a few values before and after for context
        before = s32(data, h - 8) if h >= 8 else 0
        before2 = s32(data, h - 4) if h >= 4 else 0
        print(f"    0x{h:05X}  (preceding values: {before2}, {before})")

    # --- Float 1.0 pairs ---
    print("\n--- Float 1.0 pairs followed by coordinate-range values ---")
    f10 = struct.pack('<f', 1.0)
    pattern_f = f10 + f10
    results = []
    for off in range(0, len(data) - 16, 4):
        if data[off:off+8] == pattern_f:
            x = s32(data, off + 8)
            y = s32(data, off + 12)
            if 0 <= x <= 2000 and 0 <= y <= 2000 and (x > 0 or y > 0):
                results.append((off, x, y))

    print(f"  Found {len(results)} (1.0, 1.0, X, Y) patterns:")
    for off, x, y in results:
        print(f"    0x{off:05X}: pos=({x:5d}, {y:5d})")

    # --- FLAG value 0x80000000 ---
    print(f"\n--- FLAG (0x80000000) occurrences ---")
    flag = struct.pack('<I', 0x80000000)
    hits = []
    pos = 0
    while True:
        p = data.find(flag, pos)
        if p == -1:
            break
        if p % 4 == 0:  # Only 4-byte aligned
            hits.append(p)
        pos = p + 1
    print(f"  Found {len(hits)} aligned occurrences:")
    for h in hits[:30]:
        print(f"    0x{h:05X}")
    if len(hits) > 30:
        print(f"    ... and {len(hits)-30} more")

    print()
    return results


# ============================================================
# Phase 5: Stride detection
# ============================================================

def detect_strides(float_results):
    print("=" * 70)
    print("PHASE 5: STRIDE DETECTION")
    print("=" * 70)

    if len(float_results) < 3:
        print("  Not enough float pair results to detect strides.")
        print()
        return []

    # Group results by common stride
    # Try all consecutive pairs and collect stride frequencies
    stride_counts = {}
    for i in range(1, len(float_results)):
        stride = float_results[i][0] - float_results[i-1][0]
        if stride > 0 and stride < 10000:
            stride_counts[stride] = stride_counts.get(stride, 0) + 1

    # Sort by frequency
    common_strides = sorted(stride_counts.items(), key=lambda x: -x[1])

    print("Most common strides between (1.0, 1.0, X, Y) entries:")
    record_groups = []
    for stride, count in common_strides[:10]:
        print(f"  Stride {stride} (0x{stride:X}): {count} occurrences")

        # Find the longest run of this stride
        best_run = []
        current_run = [float_results[0]]
        for i in range(1, len(float_results)):
            if float_results[i][0] - float_results[i-1][0] == stride:
                current_run.append(float_results[i])
            else:
                if len(current_run) > len(best_run):
                    best_run = list(current_run)
                current_run = [float_results[i]]
        if len(current_run) > len(best_run):
            best_run = current_run

        if len(best_run) >= 3:
            print(f"    Longest consecutive run: {len(best_run)} entries")
            print(f"    First: 0x{best_run[0][0]:05X} ({best_run[0][1]}, {best_run[0][2]})")
            print(f"    Last:  0x{best_run[-1][0]:05X} ({best_run[-1][1]}, {best_run[-1][2]})")
            record_groups.append((stride, best_run))

    print()
    return record_groups


# ============================================================
# Phase 6: Record comparison
# ============================================================

def compare_records(data, record_groups):
    print("=" * 70)
    print("PHASE 6: RECORD COMPARISON")
    print("=" * 70)

    for stride, entries in record_groups:
        if len(entries) < 3:
            continue

        print(f"\n--- Record group: {len(entries)} entries, stride={stride} (0x{stride:X}) ---")

        # The float pair is somewhere within each record.
        # The values at -12 and -8 relative to the float were the positions
        # in the job tree UIB. Let's check several offsets relative to the float pair.

        # Compute the record "base" as: first_float_offset - (stride - stride)
        # Actually, let's just look at offsets relative to each float pair location.

        print(f"\nValues at fixed offsets relative to each (1.0, 1.0) marker:")
        print(f"{'Entry':>5s}", end="")
        check_offsets = [-20, -16, -12, -8, -4, 0, 4, 8, 12, 16, 20, 24]
        for off in check_offsets:
            print(f"  {off:>+4d}", end="")
        print()
        print("-" * (6 + len(check_offsets) * 6))

        for i, (addr, x, y) in enumerate(entries):
            print(f"  {i:3d}", end="")
            for off in check_offsets:
                abs_off = addr + off
                if 0 <= abs_off < len(data) - 4:
                    v = s32(data, abs_off)
                    if v == 0:
                        print(f"  {'0':>4s}", end="")
                    elif abs(v) > 100000:
                        print(f"  {'...':>4s}", end="")
                    else:
                        print(f"  {v:4d}", end="")
                else:
                    print(f"  {'?':>4s}", end="")
            print()

        # Identify which columns vary (potential per-instance data)
        print(f"\nColumn variability analysis:")
        for off in check_offsets:
            values = set()
            for addr, _, _ in entries:
                abs_off = addr + off
                if 0 <= abs_off < len(data) - 4:
                    values.add(s32(data, abs_off))
            if len(values) == 1:
                val = list(values)[0]
                print(f"  Offset {off:+4d}: CONSTANT = {val}")
            else:
                vals = sorted(values)
                in_range = all(0 <= v <= 2000 for v in vals)
                note = " <- POSSIBLE COORDINATES" if in_range and len(values) > 3 else ""
                print(f"  Offset {off:+4d}: VARIES ({len(values)} unique values, "
                      f"range {min(vals)}..{max(vals)}){note}")

    print()


# ============================================================
# Phase 7: Full record dump for the most promising group
# ============================================================

def dump_best_group(data, record_groups):
    print("=" * 70)
    print("PHASE 7: DETAILED RECORD DUMP (best candidate group)")
    print("=" * 70)

    if not record_groups:
        print("  No record groups to dump.")
        print()
        return

    # Pick the group with the most entries
    best = max(record_groups, key=lambda g: len(g[1]))
    stride, entries = best

    print(f"Group: {len(entries)} entries, stride={stride}")
    print()

    # Dump the full record for the first, second, and last entry
    samples = [0, 1, len(entries) - 1] if len(entries) > 2 else list(range(len(entries)))

    for sample_idx in samples:
        addr, x, y = entries[sample_idx]
        # Record probably starts at addr - some_offset, extends for stride bytes
        # Let's dump from addr - stride + stride (just the region around the float pair)
        rec_start = addr - (stride - 4)  # Start from well before the float pair
        if rec_start < 0:
            rec_start = 0

        print(f"--- Entry {sample_idx}: float pair at 0x{addr:05X}, values=({x}, {y}) ---")
        print(f"{'Offset':>8s}  {'AbsAddr':>8s}  {'Int32':>10s}  {'Hex':>10s}  {'Float':>10s}  Notes")
        print("-" * 75)

        for i in range(0, stride, 4):
            off = rec_start + i
            if off + 4 > len(data):
                break
            v = s32(data, off)
            uv = u32(data, off)
            fv = f32(data, off)
            rel = off - addr  # Relative to the float pair

            notes = []
            if off == addr:
                notes.append("FLOAT 1.0 (scaleX)")
            elif off == addr + 4:
                notes.append("FLOAT 1.0 (scaleY)")
            elif off == addr + 8:
                notes.append(f"post-float val ({v})")
            elif off == addr + 12:
                notes.append(f"post-float val ({v})")
            if v == 1920:
                notes.append("SCREEN_WIDTH")
            if v == 1080:
                notes.append("SCREEN_HEIGHT")
            if v == 960:
                notes.append("HALF_WIDTH")
            if v == 540:
                notes.append("HALF_HEIGHT")
            if uv == 0x80000000:
                notes.append("FLAG")
            if abs(fv) < 100 and abs(fv) > 0.001 and uv > 0x100:
                notes.append(f"float?={fv:.3f}")

            float_str = ""
            if abs(fv) < 10000 and abs(fv) > 0.001 and uv > 0x100:
                float_str = f"{fv:10.4f}"

            note_str = "  ".join(notes)
            if v != 0:
                print(f"  {rel:>+5d}   0x{off:05X}  {v:10d}  0x{uv:08X}  {float_str:>10s}  {note_str}")

        print()


# ============================================================
# Phase 8: Search for specific value pairs
# ============================================================

def search_value_pairs(data):
    print("=" * 70)
    print("PHASE 8: NOTABLE VALUE PAIRS")
    print("=" * 70)
    print("Pairs of consecutive 4-byte values where both are in")
    print("coordinate range (50-1800) and differ from each other:")
    print()

    pairs = []
    for off in range(0, len(data) - 8, 4):
        a = s32(data, off)
        b = s32(data, off + 4)
        if 50 <= a <= 1800 and 50 <= b <= 1800 and a != b:
            pairs.append((off, a, b))

    # Deduplicate nearby hits (within 8 bytes of each other)
    filtered = []
    for p in pairs:
        if not filtered or p[0] - filtered[-1][0] >= 8:
            filtered.append(p)

    print(f"Found {len(filtered)} candidate pairs:")
    for off, a, b in filtered[:60]:
        # Check what's before and after
        before = s32(data, off - 4) if off >= 4 else 0
        after = s32(data, off + 8) if off + 8 < len(data) else 0
        print(f"  0x{off:05X}: ({a:5d}, {b:5d})  context: [{before}, {a}, {b}, {after}]")

    if len(filtered) > 60:
        print(f"  ... and {len(filtered) - 60} more pairs")
    print()


# ============================================================
# Main
# ============================================================

def main():
    if len(sys.argv) < 2:
        print(__doc__)
        print("Error: Please provide a .uib file path as an argument.")
        print("Example: python analyze_uib.py ffto_job_tree.uib")
        sys.exit(1)

    filepath = sys.argv[1]
    if not os.path.exists(filepath):
        print(f"Error: File not found: {filepath}")
        sys.exit(1)

    with open(filepath, 'rb') as f:
        data = f.read()

    print(f"Analyzing: {filepath}")
    print(f"{'=' * 70}\n")

    # Phase 1: Header
    analyze_header(data)

    # Phase 2: String tables
    string_tables = find_string_tables(data)

    # Phase 3: String references
    find_string_references(data, string_tables)

    # Phase 4: Known patterns
    float_results = search_known_patterns(data)

    # Phase 5: Stride detection
    record_groups = detect_strides(float_results)

    # Phase 6: Record comparison
    compare_records(data, record_groups)

    # Phase 7: Detailed dump
    dump_best_group(data, record_groups)

    # Phase 8: Value pairs
    search_value_pairs(data)

    print("=" * 70)
    print("ANALYSIS COMPLETE")
    print("=" * 70)
    print()
    print("Next steps:")
    print("  1. Look at the string tables for widget/element names")
    print("  2. Check the stride detection results for record arrays")
    print("  3. Look at the 'VARIES' columns in record comparison for positions")
    print("  4. Cross-reference coordinate candidates with in-game screenshots")
    print("  5. Try modifying a candidate value, repack, and test in-game")


if __name__ == '__main__':
    main()
