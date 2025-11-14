let cachedConfig = null;
async function getConfig() {
    if (cachedConfig) {
        return cachedConfig;
    }
    try {
        const response = await fetch('/config');
        if (!response.ok) {
            throw new Error(`Config request failed: ${response.status}`);
        }
        const contentType = response.headers.get('content-type');
        if (!contentType || !contentType.includes('application/json')) {
            throw new Error('Config endpoint did not return JSON (likely in dev mode)');
        }
        cachedConfig = await response.json();
        return cachedConfig;
    }
    catch (error) {
        console.error('Failed to load configuration, using fallback:', error);
        // Fallback configuration for development mode
        cachedConfig = {
            apiServiceBaseUrl: import.meta.env.VITE_API_BASE_URL || 'http://localhost:8080'
        };
        return cachedConfig;
    }
}
async function resolveBaseUrl() {
    const config = await getConfig();
    let baseUrl = config.apiServiceBaseUrl;
    // If the current page is served over HTTPS, ensure API calls also use HTTPS
    if (window.location.protocol === 'https:' && baseUrl.startsWith('http://')) {
        baseUrl = baseUrl.replace('http://', 'https://');
        console.log("Upgraded API URL to HTTPS for mixed content compliance:", baseUrl);
    }
    console.log("Resolved APIService Base URL from config:", baseUrl);
    return baseUrl;
}
export async function getWeather(maxItems = 10) {
    const baseUrl = (await resolveBaseUrl())?.replace(/\/$/, ''); // strip trailing slash for consistency
    const resp = await fetch(`${baseUrl}/weatherforecast`);
    if (!resp.ok) {
        throw new Error(`Weather request failed: ${resp.status}`);
    }
    const data = await resp.json();
    return data.slice(0, maxItems);
}
