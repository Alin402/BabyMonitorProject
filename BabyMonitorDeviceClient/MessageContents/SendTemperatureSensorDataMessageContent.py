class SendTemperatureSensorDataMessageContent:
    def __init__(self, userID, temperatureF, temperatureC, humidity):
        self.UserID = userID
        self.TemperatureF = temperatureF
        self.TemperatureC = temperatureC
        self.Humidity = humidity

    def __json__(self):
        return {
            "UserID": self.UserID,
            "TemperatureF": self.TemperatureF,
            "TemperatureC": self.TemperatureC,
            "Humidity": self.Humidity
        }