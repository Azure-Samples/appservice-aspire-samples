import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useEffect, useState } from 'react';
import { getWeather } from '../services/weatherApi';
const Weather = () => {
    const [forecasts, setForecasts] = useState(null);
    const [error, setError] = useState(null);
    useEffect(() => {
        getWeather().then(setForecasts).catch(e => setError(e.message));
    }, []);
    return (_jsxs("div", { children: [_jsx("h1", { children: "Weather" }), _jsx("p", { children: "This component demonstrates showing data loaded from the python backend API service." }), error && _jsxs("p", { style: { color: 'red' }, children: ["Error: ", error] }), forecasts === null && !error && _jsx("p", { children: _jsx("em", { children: "Loading..." }) }), forecasts && (_jsxs("table", { className: "table", children: [_jsx("thead", { children: _jsxs("tr", { children: [_jsx("th", { children: "Date" }), _jsx("th", { "aria-label": "Temperature in Celsius", children: "Temp. (C)" }), _jsx("th", { "aria-label": "Temperature in Fahrenheit", children: "Temp. (F)" }), _jsx("th", { children: "Summary" })] }) }), _jsx("tbody", { children: forecasts.map(f => (_jsxs("tr", { children: [_jsx("td", { children: new Date(f.date).toLocaleDateString() }), _jsx("td", { children: f.temperatureC }), _jsx("td", { children: f.temperatureF }), _jsx("td", { children: f.summary })] }, f.date))) })] }))] }));
};
export default Weather;
