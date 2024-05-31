def get_connect_to_server_message(connectToServerMessageContent):
    return {
        "ClientType": 1,
        "MessageType": 0,
        "Content": connectToServerMessageContent.__json__()
    }


def get_send_temperature_sensor_data_message(sendTemperatureSensorDataMessageContent):
    return {
        "ClientType": 1,
        "MessageType": 4,
        "Content": sendTemperatureSensorDataMessageContent.__json__()
    }


def get_send_system_data_message(sendSystemDataMessageContent):
    return {
        "ClientType": 1,
        "MessageType": 5,
        "Content": sendSystemDataMessageContent.__json__()
    }
