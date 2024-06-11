#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <DHT.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";

const char* serverUrl = "https://5373-46-229-61-72.ngrok-free.app/api/Sensor/42";

#define DHTPIN 23
#define DHTTYPE DHT22

DHT dht(DHTPIN, DHTTYPE);

const int lampPin = 5;
bool isLampOn = false;

float temp;

const int minHumidity = 40;
const int maxHumidity = 60;
const int minTemperature = 17;
const int maxTemperature = 22;

LiquidCrystal_I2C lcd(0x27, 16, 2);

void connectToWiFi() {
  Serial.print("Connecting to WiFi");
  while (WiFi.status() != WL_CONNECTED) {
    WiFi.begin(ssid, password);
    delay(1000);
    Serial.print(".");
  }
  Serial.println("\nConnected to WiFi");
  Serial.print("IP Address: ");
  Serial.println(WiFi.localIP());
}

void setup() {
  Serial.begin(115200);
  connectToWiFi();

  dht.begin();

  pinMode(lampPin, OUTPUT);

  Wire.begin(21, 22);

  lcd.begin(16, 2);
}

void displayData(float temperature, float humidity) {
  if (temperature > minTemperature && temperature < maxTemperature) {
    Serial.println("Temperature: Normal");
  } else if (temperature > maxTemperature) {
    Serial.print("The temperature is higher by ");
    Serial.print(temperature - maxTemperature);
    Serial.println("C");
  } else {
    Serial.print("The temperature is lower by ");
    Serial.print(minTemperature - temperature);
    Serial.println("C");
  }
  
  if (humidity >= minHumidity && humidity <= maxHumidity) {
    Serial.println("Humidity: Normal");
  } else if (humidity > maxHumidity) {
    Serial.print("The humidity is higher by ");
    Serial.print(humidity - maxHumidity);
    Serial.println("%");
  } else {
    Serial.print("The humidity is lower by ");
    Serial.print(minHumidity - humidity);
    Serial.println("%");
  }

  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print("Temperature: ");
  lcd.print(temperature);
  lcd.println(" C");

  lcd.setCursor(0, 1);
  lcd.print("Humidity: ");
  lcd.print(humidity);
  lcd.println(" %");
}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    float temperature = dht.readTemperature();
    float humidity = dht.readHumidity();

    if (isnan(temperature) || isnan(humidity)) {
      Serial.println("Failed to read from DHT sensor!");
      return;
    }

    displayData(temperature, humidity);

    StaticJsonDocument<200> jsonDoc;
    jsonDoc["temperature"] = temperature;
    jsonDoc["humidity"] = humidity;

    String jsonStr;
    serializeJson(jsonDoc, jsonStr);

    HTTPClient http;
    http.begin(serverUrl);

    http.addHeader("Content-Type", "application/json");

    int httpResponseCode = http.PUT(jsonStr);

    if (httpResponseCode > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpResponseCode);
    } else {
      Serial.print("Error code: ");
      Serial.println(httpResponseCode);
    }

    http.end();

    if (temperature < minTemperature || temperature > maxTemperature || humidity < minHumidity || humidity > maxHumidity) {
      if (!isLampOn) {
        digitalWrite(lampPin, HIGH);
        isLampOn = true;
      }
    } else {
      if (isLampOn) {
        digitalWrite(lampPin, LOW);
        isLampOn = false;
      }
    }
  } else {
    Serial.println("WiFi Disconnected. Reconnecting...");
    connectToWiFi();
  }

  delay(10000);
}
