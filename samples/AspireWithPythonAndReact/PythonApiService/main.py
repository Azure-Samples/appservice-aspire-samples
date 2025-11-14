import os
from datetime import date, timedelta
from flask import Flask, jsonify
from flask_cors import CORS

app = Flask(__name__)

# CORS configuration from environment variable ALLOWED_ORIGINS.
# Comma-separated list. Empty or '*' allows all origins (useful for local dev, not prod).
_origins_raw = os.getenv("ALLOWED_ORIGINS", "").strip()
if not _origins_raw or _origins_raw == "*":
    allowed_origins = "*"
else:
    origins_list = []
    for origin in _origins_raw.split(","):
        origin = origin.strip()
        if origin:
            origins_list.append(origin)
            if origin.startswith("http://"):
                origins_list.append(origin.replace("http://", "https://"))
            elif origin.startswith("https://"):
                origins_list.append(origin.replace("https://", "http://"))
    allowed_origins = list(set(origins_list))  # Remove duplicates

# Apply CORS to all routes.
CORS(app, resources={r"/*": {"origins": allowed_origins}})

@app.get("/")
def root():
    return jsonify(message="Hello from Python Flask API!")

@app.get("/health")
def health():
    return jsonify(status="healthy")

@app.get("/weatherforecast")
def weather():
    summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"]
    base = date.today()
    data = []
    for i in range(5):
        c = (-20 + i * 7)
        data.append({
            "date": (base + timedelta(days=i)).isoformat(),
            "temperatureC": c,
            "temperatureF": int(c * 9/5 + 32),
            "summary": summaries[i % len(summaries)]
        })
    return jsonify(data)

def create_app():
    return app

if __name__ == "__main__":
    port = int(os.environ.get("PORT", "8080"))
    app.run(host="0.0.0.0", port=port, debug=False, use_reloader=False)