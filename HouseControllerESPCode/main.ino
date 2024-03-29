#include <ArduinoJson.h>
#include <ArduinoJson.hpp>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <WiFiServer.h>
#include <WiFiUdp.h>
#include <ArduinoOTA.h>

WiFiServer server(2500);
WiFiUDP udpServer;
DynamicJsonDocument initialData(500);
char initialDataToSend[2048];

const char SSID[] = "VIGILO";
const char PASS[] = "brian12321";
const char HOSTNAME[] = "HouseControllerESP";

const char CHECKDEVICE_TYPE[] = "1";
const char INITIALDATA_TYPE[] = "InDt";
const char UPDATE_TYPE[] = "Update";
const char NAME_TYPE[] = "Name";
const char STATUS_TYPE[] = "Status";
const char TIMEINFOLIST_TYPE[] = "TimeInfoList";
const char TIME_TYPE[] = "Time";
const char TIMESTATUS_TYPE[] = "TimeStatus";

const int MaxDevices = 10;
WiFiClient clients[MaxDevices];
void setup() {
  //Set all the pins to output to control Devices
  for(int i=0;i<2;i++){
      pinMode(i, OUTPUT);
  }
  // Serial.begin(9600);
  WiFi.begin(SSID, PASS);
  WiFi.hostname(HOSTNAME);
  //Wait until the Wifi is connected
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  setupJson();
  Serial.println(initialDataToSend);
  Serial.println(WiFi.getHostname());
  server.begin();
  udpServer.begin(2500);
}

void setupJson() {
  initialData[0]["Name"] = "testa";
  initialData[0]["Id"] = 0;
  initialData[0]["Status"] = 0;
  initialData[0]["TimeInfoList"][0]["Time"] = "9:00";
  initialData[0]["TimeInfoList"][1]["Time"] = "10:00";
  initialData[0]["TimeInfoList"][2]["Time"] = "12:00";
  initialData[0]["TimeInfoList"][3]["Time"] = "16:00";
  initialData[0]["TimeInfoList"][4]["Time"] = "17:00";
  initialData[0]["TimeInfoList"][5]["Time"] = "11:00";
  initialData[0]["TimeInfoList"][0]["TimeStatus"] = 0;
  initialData[0]["TimeInfoList"][1]["TimeStatus"] = 1;
  initialData[0]["TimeInfoList"][2]["TimeStatus"] = 1;
  initialData[0]["TimeInfoList"][3]["TimeStatus"] = 0;
  initialData[0]["TimeInfoList"][4]["TimeStatus"] = 0;
  initialData[0]["TimeInfoList"][5]["TimeStatus"] = 1;
  initialData[1]["Name"] = "Prueba";
  initialData[1]["Id"] = 1;
  initialData[1]["Status"] = 1;
  initialData[1]["TimeInfoList"][0]["Time"] = "9:00";
  initialData[1]["TimeInfoList"][1]["Time"] = "10:00";
  initialData[1]["TimeInfoList"][0]["TimeStatus"] = 0;
  initialData[1]["TimeInfoList"][1]["TimeStatus"] = 1;
  serializeJson(initialData, initialDataToSend);
}

void append(char* sm, char c){
  int len = strlen(sm);
  sm[len] = c;
  sm[len+1] = '\0';
}

int devicesConnected = 0;
void loop() {
  WiFiClient client = server.accept();
  int udpSize = udpServer.parsePacket();
  for(int i=0;i<2;i++){
    if(initialData[i]["Status"] == 1){
      digitalWrite(i, HIGH);
    }
    else{
      digitalWrite(i, LOW);
    }
  }
  if(udpSize > 0)
  {
    while(udpServer.available() > 0){
      Serial.println(udpServer.available());
      Serial.println("SIZEEE");
      Serial.println(udpSize);
      char data[12];
      int len = udpServer.read(data, 12);
      data[len] = 0;
      if(strcmp(data, "1") == 0){
        udpServer.beginPacket(udpServer.remoteIP(), udpServer.remotePort());
        udpServer.write("1");
        udpServer.endPacket();
        Serial.println("Sent");
      }
      Serial.println(data);
    }
  }
  if(client){
    for(int i=0;i<MaxDevices;i++){
      if(clients[i] == 1) continue;
      clients[i] = client;
      devicesConnected +=1;
      break;
    }
  }
  for(int i=0;i<MaxDevices;i++){
    WiFiClient client = clients[i];
    if(client == 0 | client.available() == 0) continue;
      String data;
      while(client.available() > 0){
        char c = client.read();
        data += c;
      }
      char dataChars[2048];
      data.toCharArray(dataChars, 2048);
      retrieveDataToDevice(client, dataChars);
  }
}

const char DataDelimiter[] = ";";
const char TimeInfoDelimiter[] = "-";
void retrieveDataToDevice(WiFiClient client, char* dataReceived){
  if(strcmp(dataReceived, CHECKDEVICE_TYPE) == 0){
    client.println("isDevice");
    Serial.println("Yes");
    return;
  }
  if(strcmp(dataReceived, INITIALDATA_TYPE) == 0){
    client.println(initialDataToSend);
    return;
  }
  char *savePtr = NULL;
  char *token = strtok_r(dataReceived, DataDelimiter, &savePtr);
  if(strcmp(token, UPDATE_TYPE) == 0){
    int id = atoi(strtok_r(NULL, DataDelimiter, &savePtr));
    Serial.println(id);
    char *dataType = strtok_r(NULL, DataDelimiter, &savePtr);
    Serial.println(dataType);
    char *dataValue = strtok_r(NULL, DataDelimiter, &savePtr);
    Serial.println(dataValue);
    if(strcmp(dataType, NAME_TYPE) == 0){
      initialData[id][NAME_TYPE] = dataValue;
    }
    else if(strcmp(dataType, STATUS_TYPE) == 0){
      initialData[id][STATUS_TYPE] = atoi(dataValue);
    }
    else if(strcmp(dataType, TIMEINFOLIST_TYPE) == 0){
      int index = atoi(strtok_r(dataValue, TimeInfoDelimiter, &savePtr));
      char *time = strtok_r(NULL, TimeInfoDelimiter, &savePtr);
      int timeStatus = atoi(strtok_r(NULL, TimeInfoDelimiter, &savePtr));
      initialData[id][TIMEINFOLIST_TYPE][index][TIME_TYPE] = time;
      initialData[id][TIMEINFOLIST_TYPE][index][TIMESTATUS_TYPE] = timeStatus;
      snprintf(dataValue, 200, "%d-%s-%d", index, time, timeStatus);
    }
    serializeJson(initialData, initialDataToSend);
    char datatosend[256];
    snprintf(datatosend, 256, "Updated;%d;%s;%s", id, dataType, dataValue);
    SendDataToAllDevices(client, datatosend);
  }
}

void SendDataToAllDevices(WiFiClient client, char* data){
  for(int i=0;i<MaxDevices;i++){
    WiFiClient client = clients[i];
    if(client == 0) continue;
    client.println(data);
    Serial.println(data);
  }
}
