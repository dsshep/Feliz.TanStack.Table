import { Union } from "../fable_modules/fable-library.3.7.1/Types.js";
import { union_type, list_type, obj_type, class_type, lambda_type, string_type } from "../fable_modules/fable-library.3.7.1/Reflection.js";

export class ColumnDefOption$1 extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Id", "AccessorKey", "AccessorFn", "Header", "HeaderFn", "Footer", "FooterFn", "Cell", "Columns"];
    }
}

export function ColumnDefOption$1$reflection(gen0) {
    return union_type("Feliz.TanStack.Table.Types.ColumnDefOption`1", [gen0], ColumnDefOption$1, () => [[["Item", string_type]], [["Item", string_type]], [["Item", lambda_type(gen0, string_type)]], [["Item", string_type]], [["Item", lambda_type(class_type("Feliz.TanStack.Table.Types.HeaderFnProps`1", [gen0]), obj_type)]], [["Item", string_type]], [["Item", lambda_type(class_type("Feliz.TanStack.Table.Types.HeaderFnProps`1", [gen0]), obj_type)]], [["Item", lambda_type(class_type("Feliz.TanStack.Table.Types.CellContext`1", [gen0]), class_type("Fable.React.ReactElement"))]], [["Item", list_type(list_type(ColumnDefOption$1$reflection(gen0)))]]]);
}

