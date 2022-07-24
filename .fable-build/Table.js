import { Record } from "./fable_modules/fable-library.3.7.1/Types.js";
import { record_type, int32_type, string_type } from "./fable_modules/fable-library.3.7.1/Reflection.js";
import { ofArray } from "./fable_modules/fable-library.3.7.1/List.js";
import { ColumnDefOption$1 } from "./Feliz.TanStack.Table/Types.js";
import { createElement } from "react";
import { createTanStackTable } from "./Feliz.TanStack.Table/TanStackTable.js";

export class Person extends Record {
    constructor(Firstname, Lastname, Age, Visits, Status, Progress) {
        super();
        this.Firstname = Firstname;
        this.Lastname = Lastname;
        this.Age = (Age | 0);
        this.Visits = (Visits | 0);
        this.Status = Status;
        this.Progress = (Progress | 0);
    }
}

export function Person$reflection() {
    return record_type("Table.Person", [], Person, () => [["Firstname", string_type], ["Lastname", string_type], ["Age", int32_type], ["Visits", int32_type], ["Status", string_type], ["Progress", int32_type]]);
}

export const defaultData = ofArray([new Person("tanner", "linsley", 24, 100, "In Relationship", 50), new Person("tandy", "miller", 40, 40, "Single", 80), new Person("joe", "dirte", 45, 20, "Complicated", 10)]);

export const columnDef = ofArray([ofArray([new ColumnDefOption$1(0, "firstname"), new ColumnDefOption$1(1, "Firstname"), new ColumnDefOption$1(7, (info) => info.getValue())]), ofArray([new ColumnDefOption$1(0, "lastName"), new ColumnDefOption$1(1, "Lastname"), new ColumnDefOption$1(7, (info_1) => createElement("i", {
    children: info_1.getValue(),
})), new ColumnDefOption$1(4, (_arg1) => createElement("span", {
    children: "Last Name",
})), new ColumnDefOption$1(6, (info_2) => info_2.column.id)]), ofArray([new ColumnDefOption$1(1, "Age"), new ColumnDefOption$1(4, (_arg2) => "Age"), new ColumnDefOption$1(7, (info_3) => info_3.renderValue()), new ColumnDefOption$1(6, (info_4) => info_4.column.id)]), ofArray([new ColumnDefOption$1(1, "Visits"), new ColumnDefOption$1(4, (_arg3) => createElement("span", {
    children: "Visits",
})), new ColumnDefOption$1(6, (info_5) => info_5.column.id)]), ofArray([new ColumnDefOption$1(1, "Status"), new ColumnDefOption$1(3, "Status"), new ColumnDefOption$1(6, (info_6) => info_6.column.id)]), ofArray([new ColumnDefOption$1(1, "Progress"), new ColumnDefOption$1(3, "Profile Progress"), new ColumnDefOption$1(6, (info_7) => info_7.column.id)])]);

export function createTable(render) {
    return createElement(createTanStackTable, {
        data: defaultData,
        columnDefs: columnDef,
        render: render,
    });
}

