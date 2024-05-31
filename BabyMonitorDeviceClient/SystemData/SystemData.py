import subprocess
import json
import asyncio
from MessageContents.SendSystemDataContent import SendSystemDataContent
from Messages.Messages import get_send_system_data_message


def get_current_wifi_name():
    try:
        result = subprocess.check_output(["iwgetid", "-r"])
        return result.strip().decode("utf-8")
    except subprocess.CalledProcessError:
        return "Not connected to Wi-Fi"


async def send_system_data(websocket, appData, event, lock):
    userID = appData.UserID
    wifiName = get_current_wifi_name()
    with lock:
        while event.is_set():
            try:
                with open("/sys/class/thermal/thermal_zone0/temp", "r") as file:
                    if file is None:
                        await asyncio.sleep(2)
                        continue
                    temperature_str = file.readline()
                    temperature = float(temperature_str) / 1000.0
                    print("sending system temperature..." + str(temperature))
                    messageContent = SendSystemDataContent(userID, temperature, wifiName)
                    message = get_send_system_data_message(messageContent)
                    if websocket.open:
                        await websocket.send(json.dumps(message))
                await asyncio.sleep(2)
            except Exception as e:
                print("Exception in system temperature: " + str(e))
                await asyncio.sleep(2)
                continue
