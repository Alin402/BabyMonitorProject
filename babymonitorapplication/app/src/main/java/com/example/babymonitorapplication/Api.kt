package com.example.babymonitorapplication
import TokenManager
import android.content.Context
import com.google.gson.Gson
import com.google.gson.JsonArray
import kotlinx.serialization.Serializable
import kotlinx.serialization.Serializer
import kotlinx.serialization.decodeFromString
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonElement
import kotlinx.serialization.json.decodeFromJsonElement
import kotlinx.serialization.json.jsonArray
import kotlinx.serialization.json.jsonObject
import kotlinx.serialization.json.jsonPrimitive
import okhttp3.Call
import okhttp3.Callback
import okhttp3.Cookie
import okhttp3.CookieJar
import okhttp3.FormBody
import okhttp3.HttpUrl
import okhttp3.HttpUrl.Companion.toHttpUrlOrNull
import okhttp3.MediaType
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.toRequestBody
import okhttp3.Response
import org.json.JSONArray
import java.io.IOException
import java.util.Date

class SimpleCookieJar : CookieJar {
    private val cookieStore: MutableMap<String, List<Cookie>> = mutableMapOf()

    override fun saveFromResponse(url: HttpUrl, cookies: List<Cookie>) {
        cookieStore[url.host] = cookies
    }

    override fun loadForRequest(url: HttpUrl): List<Cookie> {
        return cookieStore[url.host] ?: listOf()
    }

    fun clearCookies() {
        cookieStore.clear()
    }
}

@Serializable
data class User(val email: String, val username: String, val name: String, val password: String)
@Serializable
data class AuthUser(val email: String, val password: String)

class Api() {
    companion object {
        private const val apiUrl : String = "http://68.219.120.90:5000/api/"

        fun loginAndGetCookies(url: String, email: String, password: String, context: Context, callback: (Boolean) -> Unit) {
            val simpleCookieJar = SimpleCookieJar()
            val client = OkHttpClient.Builder()
                .cookieJar(simpleCookieJar)
                .build()

            val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()
            val json = Json.encodeToString(AuthUser(email, password))

            val request = Request.Builder()
                .url(apiUrl + url)
                .post(json.toRequestBody(mediaType))
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false)
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string()
                    println("Response body: " + responseBody)

                    if (response.isSuccessful) {
                        // Save cookies to the cookie jar
                        val cookies = Cookie.parseAll(request.url, response.headers)
                        if (cookies != null) {
                            println("Cookies: " + cookies[0]);
                            val tokenManager  = TokenManager(context);
                            tokenManager.saveToken(cookies[0].toString());
                        }

                        callback(true)
                    } else {
                        callback(false)
                    }
                }
            })
        }

        fun signup(url: String, name: String, username: String, email: String, password: String, callback: (Boolean) -> Unit) {
            val client = OkHttpClient.Builder()
                .cookieJar(SimpleCookieJar())
                .build()

            val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()

            val json = Json.encodeToString(User(email, username, name, password))

            val request = Request.Builder()
                .url(apiUrl + url)
                .post(json.toRequestBody(mediaType))
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false)
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string();
                    println("Response body: " + responseBody);

                    if (response.isSuccessful) {
                        callback(true)
                    } else {
                        callback(false)
                    }
                }
            })
        }

        fun getUser(url: String, context: Context, callback: (Boolean) -> Unit) {
            val simpleCookieJar = SimpleCookieJar()
            val client = OkHttpClient.Builder()
                .cookieJar(simpleCookieJar)
                .build()

            val tokenManager = TokenManager(context);
            val cookie = tokenManager.getToken();

            val request = Request.Builder()
                .addHeader("Cookie", cookie.toString())
                .url(apiUrl + url)
                .get()
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false)
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string();
                    println("Response body: " + responseBody);

                    if (response.isSuccessful) {
                        callback(true)
                    } else {
                        callback(false)
                    }
                }
            })
        }

        fun logout(url: String, context: Context, callback: (Boolean) -> Unit) {
            val simpleCookieJar = SimpleCookieJar()
            val client = OkHttpClient.Builder()
                .cookieJar(simpleCookieJar)
                .build()

            val tokenManager = TokenManager(context);
            val cookie = tokenManager.getToken();

            val request = Request.Builder()
                .addHeader("Cookie", cookie.toString())
                .url(apiUrl + url)
                .get()
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false)
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string();
                    println("Response body: " + responseBody);

                    if (response.isSuccessful) {
                        callback(true)
                    } else {
                        callback(false)
                    }
                }
            })
        }

        @Serializable
        data class Baby(val id: String, val name: String, val userId: String, val photoUrl: String, val birthDate: String)

        fun getBabies(url: String, context: Context, callback: (Boolean, List<Baby>) -> Unit) {
            val simpleCookieJar = SimpleCookieJar()
            val client = OkHttpClient.Builder()
                .cookieJar(simpleCookieJar)
                .build()

            val tokenManager = TokenManager(context);
            val cookie = tokenManager.getToken();

            val request = Request.Builder()
                .addHeader("Cookie", cookie.toString())
                .url(apiUrl + url)
                .get()
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false, emptyList())
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string();
                    println("Response body: " + responseBody);

                    val jsonArray: kotlinx.serialization.json.JsonArray = Json.parseToJsonElement(responseBody).jsonArray

                    // Convert JSON array to list of maps
                    val listOfMaps: List<Map<String, JsonElement>> = jsonArray.map { it.jsonObject }

                    // Map list of maps to list of Baby objects
                    val babies: List<Baby> = listOfMaps.map {
                        Baby(
                            it["id"]!!.jsonPrimitive.content,
                            it["name"]!!.jsonPrimitive.content,
                            it["userId"]!!.jsonPrimitive.content,
                            it["photoUrl"]!!.jsonPrimitive.content,
                            it["name"]!!.jsonPrimitive.content,
                        )
                    }
                    if (response.isSuccessful) {
                        callback(true, babies)
                    } else {
                        callback(false, emptyList())
                    }
                }
            })
        }

        @Serializable
        data class Device(val id: String, val name: String, val babyId: String)

        fun registerDevice(url: String, context: Context, deviceId: String, deviceName: String, babyId: String, callback: (Boolean) -> Unit) {
            val simpleCookieJar = SimpleCookieJar()
            val client = OkHttpClient.Builder()
                .cookieJar(simpleCookieJar)
                .build()

            val tokenManager = TokenManager(context);
            val cookie = tokenManager.getToken();

            val mediaType = "application/json; charset=utf-8".toMediaTypeOrNull()

            val json = Json.encodeToString(Device(deviceId, deviceName, babyId))
            println(deviceId + " " + deviceName + " " + babyId)

            val request = Request.Builder()
                .addHeader("Cookie", cookie.toString())
                .url(apiUrl + url)
                .post(json.toRequestBody(mediaType))
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    e.printStackTrace()
                    callback(false)
                }

                override fun onResponse(call: Call, response: Response) {
                    println("Response: " + response.code)
                    val responseBody = response.body!!.string();
                    println("Response body: " + responseBody);

                    if (response.isSuccessful) {
                        callback(true)
                    } else {
                        callback(false)
                    }
                }
            })
        }
    }
}