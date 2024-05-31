class ConnectToServerMessageContent:
    def __init__(self, apiKeyId, apiKeyValue, deviceID, userID, streamingChannelUrl):
        self.ApiKeyId = apiKeyId
        self.ApiKeyValue = apiKeyValue
        self.DeviceID = deviceID
        self.UserID = userID
        self.StreamingChannelUrl = streamingChannelUrl

    def __json__(self):
        return {
            "ApiKeyId": self.ApiKeyId,
            "ApiKeyValue": self.ApiKeyValue,
            "DeviceID": self.DeviceID,
            "UserID": self.UserID,
            "StreamingChannelUrl": self.StreamingChannelUrl
        }