export interface WeatherForecast {
  date: string; // ISO date
  temperatureC: number;
  temperatureF: number; // Provided by API for convenience
  summary?: string;
}
