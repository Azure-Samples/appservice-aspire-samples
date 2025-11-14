import React from 'react';
import { Routes, Route, NavLink } from 'react-router-dom';
import Home from './pages/Home';
import Counter from './pages/Counter';
import Weather from './pages/Weather';
import './styles/layout.css';

const App: React.FC = () => {
  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand">AspireWithPythonAndReact</div>
        <nav>
          <ul>
            <li><NavLink to="/" end>Home</NavLink></li>
            <li><NavLink to="/counter">Counter</NavLink></li>
            <li><NavLink to="/weather">Weather</NavLink></li>
          </ul>
        </nav>
      </aside>
      <main className="main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/counter" element={<Counter />} />
          <Route path="/weather" element={<Weather />} />
        </Routes>
      </main>
    </div>
  );
};

export default App;
