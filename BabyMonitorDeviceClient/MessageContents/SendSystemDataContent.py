class SendSystemDataContent:
    def __init__(self, userID, systemTemperature, wifiName):
        self.UserID = userID
        self.SystemTemperature = systemTemperature
        self.WifiName = wifiName

    def __json__(self):
        return {
            "UserID": self.UserID,
            "SystemTemperature": self.SystemTemperature,
            "WifiName": self.WifiName
        }