import { Union, Record } from "./fable_modules/fable-library.3.7.1/Types.js";
import { union_type, record_type, bool_type, int32_type } from "./fable_modules/fable-library.3.7.1/Reflection.js";
import { Cmd_none } from "./fable_modules/Fable.Elmish.3.1.0/cmd.fs.js";
import { createTable } from "./Table.js";
import { map, delay, toList } from "./fable_modules/fable-library.3.7.1/Seq.js";
import { createElement } from "react";
import { createObj } from "./fable_modules/fable-library.3.7.1/Util.js";
import { flexRender } from "@tanstack/react-table";
import { Interop_reactApi } from "./fable_modules/Feliz.1.65.0/Interop.fs.js";
import { singleton, ofArray } from "./fable_modules/fable-library.3.7.1/List.js";
import { join } from "./fable_modules/fable-library.3.7.1/String.js";

export class State extends Record {
    constructor(Count, ShowTable) {
        super();
        this.Count = (Count | 0);
        this.ShowTable = ShowTable;
    }
}

export function State$reflection() {
    return record_type("Main.State", [], State, () => [["Count", int32_type], ["ShowTable", bool_type]]);
}

export class Msg extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Increment", "ShowTable"];
    }
}

export function Msg$reflection() {
    return union_type("Main.Msg", [], Msg, () => [[], []]);
}

export function init() {
    return [new State(0, false), Cmd_none()];
}

export function update(msg, state) {
    if (msg.tag === 1) {
        return [new State(state.Count, !state.ShowTable), Cmd_none()];
    }
    else {
        return [new State(state.Count + 1, state.ShowTable), Cmd_none()];
    }
}

export function view(state, dispatch) {
    let elems_8;
    const tableComponent = createTable((table) => {
        let elems_7;
        let thead;
        const children = toList(delay(() => map((headerGroup) => {
            let elems_1;
            return createElement("tr", createObj(ofArray([["key", headerGroup.id], (elems_1 = toList(delay(() => map((header) => {
                let elems;
                return createElement("th", createObj(ofArray([["key", header.id], (elems = [flexRender(header.column.columnDef.header, header.getContext())], ["children", Interop_reactApi.Children.toArray(Array.from(elems))])])));
            }, headerGroup.headers))), ["children", Interop_reactApi.Children.toArray(Array.from(elems_1))])])));
        }, table.getHeaderGroups())));
        thead = createElement("thead", {
            children: Interop_reactApi.Children.toArray(Array.from(children)),
        });
        let tbody;
        const children_2 = toList(delay(() => map((row) => {
            let elems_3;
            return createElement("tr", createObj(ofArray([["key", row.id], (elems_3 = toList(delay(() => map((cell) => {
                let elems_2, objectArg;
                return createElement("td", createObj(singleton((elems_2 = [flexRender((objectArg = cell.column.columnDef, (arg00) => objectArg.cell(arg00)), cell.getContext())], ["children", Interop_reactApi.Children.toArray(Array.from(elems_2))]))));
            }, row.getVisibleCells()))), ["children", Interop_reactApi.Children.toArray(Array.from(elems_3))])])));
        }, table.getRowModel().rows)));
        tbody = createElement("tbody", {
            children: Interop_reactApi.Children.toArray(Array.from(children_2)),
        });
        let tfoot;
        const children_4 = toList(delay(() => map((footerGroup) => {
            let elems_5;
            return createElement("tr", createObj(ofArray([["key", footerGroup.id], (elems_5 = toList(delay(() => map((footer) => {
                let elems_4;
                return createElement("th", createObj(ofArray([["key", footer.id], (elems_4 = [flexRender(footer.column.columnDef.footer, footer.getContext())], ["children", Interop_reactApi.Children.toArray(Array.from(elems_4))])])));
            }, footerGroup.headers))), ["children", Interop_reactApi.Children.toArray(Array.from(elems_5))])])));
        }, table.getFooterGroups())));
        tfoot = createElement("tfoot", {
            children: Interop_reactApi.Children.toArray(Array.from(children_4)),
        });
        return createElement("div", createObj(ofArray([["className", join(" ", ["p-2"])], (elems_7 = [createElement("table", {
            children: Interop_reactApi.Children.toArray([thead, tbody, tfoot]),
        })], ["children", Interop_reactApi.Children.toArray(Array.from(elems_7))])])));
    });
    return createElement("div", createObj(singleton((elems_8 = [createElement("p", {
        children: `Count is ${state.Count}`,
    }), createElement("button", {
        children: "Increment",
        onClick: (_arg1) => {
            dispatch(new Msg(0));
        },
    }), tableComponent], ["children", Interop_reactApi.Children.toArray(Array.from(elems_8))]))));
}

