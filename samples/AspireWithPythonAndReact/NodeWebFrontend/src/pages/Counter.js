import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState } from 'react';
const Counter = () => {
    const [count, setCount] = useState(0);
    return (_jsxs("div", { children: [_jsx("h1", { children: "Counter" }), _jsxs("p", { role: "status", children: ["Current count: ", count] }), _jsx("button", { className: "btn", onClick: () => setCount(c => c + 1), children: "Click me" })] }));
};
export default Counter;
