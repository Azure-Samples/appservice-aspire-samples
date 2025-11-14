import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Routes, Route, NavLink } from 'react-router-dom';
import Home from './pages/Home';
import Counter from './pages/Counter';
import Weather from './pages/Weather';
import './styles/layout.css';
const App = () => {
    return (_jsxs("div", { className: "app-shell", children: [_jsxs("aside", { className: "sidebar", children: [_jsx("div", { className: "brand", children: "AspireWithPythonAndReact" }), _jsx("nav", { children: _jsxs("ul", { children: [_jsx("li", { children: _jsx(NavLink, { to: "/", end: true, children: "Home" }) }), _jsx("li", { children: _jsx(NavLink, { to: "/counter", children: "Counter" }) }), _jsx("li", { children: _jsx(NavLink, { to: "/weather", children: "Weather" }) })] }) })] }), _jsx("main", { className: "main", children: _jsxs(Routes, { children: [_jsx(Route, { path: "/", element: _jsx(Home, {}) }), _jsx(Route, { path: "/counter", element: _jsx(Counter, {}) }), _jsx(Route, { path: "/weather", element: _jsx(Weather, {}) })] }) })] }));
};
export default App;
