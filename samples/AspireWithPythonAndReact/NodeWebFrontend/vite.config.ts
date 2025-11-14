import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// Determine port: prefer PORT or WEBSITES_PORT env (set by orchestrators like Aspire), fallback to 8000
const desiredPort = parseInt(process.env.PORT || process.env.WEBSITES_PORT || '8000', 10);

export default defineConfig({
  plugins: [react()],
  server: {
    port: desiredPort,
    host: '0.0.0.0',
    strictPort: true // Fail loudly if the desired port is taken instead of silently choosing another
  },
  preview: {
    port: desiredPort,
    host: '0.0.0.0'
  }
});
