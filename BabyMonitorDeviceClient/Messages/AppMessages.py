import asyncio
import json
from LifetimeOperations.RestartDevice import restart_device
from Wifi.ConnectToWifi import connect_to_wifi


async def receive_messages(websocket, appData, event, lock):
    with lock:
        print("receiving messages...")
        while event.is_set():
            try:
                message = await websocket.recv()
                jsonMessage = json.loads(message)
                messageType = jsonMessage["MessageType"]
                if messageType == 8:
                    task = asyncio.create_task(restart_device())
                    await asyncio.gather(task)
                elif messageType == 9:
                    wifiName = jsonMessage["Content"]['WifiName']
                    wifiPassword = jsonMessage["Content"]['WifiPassword']
                    connect_to_wifi(wifiName, wifiPassword)
            except Exception as e:
                print("Exception in app messages: " + str(e))
                continue
