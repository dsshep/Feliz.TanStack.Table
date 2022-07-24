import { Mocha_runTests, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.11.0/Mocha.mjs";
import { int32ToString, structuralHash, assertEqual } from "./fable_modules/fable-library.3.6.3/Util.js";
import { singleton, ofArray, contains } from "./fable_modules/fable-library.3.6.3/List.js";
import { equals, class_type, string_type, float64_type, bool_type, int32_type } from "./fable_modules/fable-library.3.6.3/Reflection.js";
import { printf, toText } from "./fable_modules/fable-library.3.6.3/String.js";

export const myFirstTest = Test_testList("Easy tests", singleton(Test_testCase("I can write and run a test...", () => {
    let copyOfStruct;
    const actual = (1 + 1) | 0;
    const expected = 2;
    const msg = "Simple test";
    if ((actual === expected) ? true : (!(new Function("try {return this===window;}catch(e){ return false;}"))())) {
        assertEqual(actual, expected, msg);
    }
    else {
        let errorMsg;
        if (contains((copyOfStruct = actual, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, class_type("System.Decimal"), class_type("System.Guid")]), {
            Equals: (x, y) => equals(x, y),
            GetHashCode: (x) => structuralHash(x),
        })) {
            const arg20 = int32ToString(actual);
            const arg10 = int32ToString(expected);
            errorMsg = toText(printf("\u003cspan style=\u0027color:black\u0027\u003eExpected:\u003c/span\u003e \u003cbr /\u003e\u003cdiv style=\u0027margin-left:20px; color:crimson\u0027\u003e%s\u003c/div\u003e\u003cbr /\u003e\u003cspan style=\u0027color:black\u0027\u003eActual:\u003c/span\u003e \u003c/br \u003e\u003cdiv style=\u0027margin-left:20px;color:crimson\u0027\u003e%s\u003c/div\u003e\u003cbr /\u003e\u003cspan style=\u0027color:black\u0027\u003eMessage:\u003c/span\u003e \u003c/br \u003e\u003cdiv style=\u0027margin-left:20px; color:crimson\u0027\u003e%s\u003c/div\u003e"))(arg10)(arg20)(msg);
        }
        else {
            errorMsg = toText(printf("\u003cspan style=\u0027color:black\u0027\u003eExpected:\u003c/span\u003e \u003cbr /\u003e\u003cdiv style=\u0027margin-left:20px; color:crimson\u0027\u003e%A\u003c/div\u003e\u003cbr /\u003e\u003cspan style=\u0027color:black\u0027\u003eActual:\u003c/span\u003e \u003c/br \u003e\u003cdiv style=\u0027margin-left:20px;color:crimson\u0027\u003e%A\u003c/div\u003e\u003cbr /\u003e\u003cspan style=\u0027color:black\u0027\u003eMessage:\u003c/span\u003e \u003c/br \u003e\u003cdiv style=\u0027margin-left:20px; color:crimson\u0027\u003e%s\u003c/div\u003e"))(expected)(actual)(msg);
        }
        throw (new Error(errorMsg));
    }
})));

Mocha_runTests(myFirstTest);

