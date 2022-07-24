import { class_type } from "../fable_modules/fable-library.3.7.1/Reflection.js";
import { useReactTable, getCoreRowModel } from "@tanstack/react-table";
import { singleton, collect, delay, toList, map, toArray } from "../fable_modules/fable-library.3.7.1/Seq.js";
import { createObj } from "../fable_modules/fable-library.3.7.1/Util.js";
import { toArray as toArray_1 } from "../fable_modules/fable-library.3.7.1/List.js";

export class prop {
    constructor() {
    }
}

export function prop$reflection() {
    return class_type("Feliz.TanStack.Table.Table.prop", void 0, prop);
}

const coreRowModel = getCoreRowModel;

function nativeColumnDefs(columnDefs) {
    return toArray(map((colDef) => createObj(toList(delay(() => collect((option) => ((option.tag === 1) ? singleton(["accessorKey", option.fields[0]]) : ((option.tag === 2) ? singleton(["accessorFn", option.fields[0]]) : ((option.tag === 3) ? singleton(["header", option.fields[0]]) : ((option.tag === 5) ? singleton(["footer", option.fields[0]]) : ((option.tag === 4) ? singleton(["header", option.fields[0]]) : ((option.tag === 6) ? singleton(["footer", option.fields[0]]) : ((option.tag === 7) ? singleton(["cell", option.fields[0]]) : ((option.tag === 8) ? singleton(["columns", nativeColumnDefs(option.fields[0])]) : singleton(["id", option.fields[0]]))))))))), colDef)))), columnDefs));
}

export function createTanStackTable(createTanStackTableInputProps) {
    const render = createTanStackTableInputProps.render;
    const columnDefs = createTanStackTableInputProps.columnDefs;
    const data = createTanStackTableInputProps.data;
    return render(useReactTable({
        columns: nativeColumnDefs(columnDefs),
        data: toArray_1(data),
        getCoreRowModel: coreRowModel(),
    }));
}

