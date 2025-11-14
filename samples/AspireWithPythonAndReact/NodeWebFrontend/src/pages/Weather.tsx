import React, { useEffect, useState } from 'react';
import { getWeather } from '../services/weatherApi';
import type { WeatherForecast } from '../types/weather';

const Weather: React.FC = () => {
  const [forecasts, setForecasts] = useState<WeatherForecast[] | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getWeather().then(setForecasts).catch(e => setError(e.message));
  }, []);

  return (
    <div>
      <h1>Weather</h1>
      <p>This component demonstrates showing data loaded from a backend API service.</p>
      {error && <p style={{color: 'red'}}>Error: {error}</p>}
      {forecasts === null && !error && <p><em>Loading...</em></p>}
      {forecasts && (
        <table className="table">
          <thead>
            <tr>
              <th>Date</th>
              <th aria-label="Temperature in Celsius">Temp. (C)</th>
              <th aria-label="Temperature in Fahrenheit">Temp. (F)</th>
              <th>Summary</th>
            </tr>
          </thead>
          <tbody>
            {forecasts.map(f => (
              <tr key={f.date}>
                <td>{new Date(f.date).toLocaleDateString()}</td>
                <td>{f.temperatureC}</td>
                <td>{f.temperatureF}</td>
                <td>{f.summary}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default Weather;
