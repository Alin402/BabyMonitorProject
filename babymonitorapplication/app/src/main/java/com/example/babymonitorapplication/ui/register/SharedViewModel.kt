package com.example.babymonitorapplication.ui.register

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.example.babymonitorapplication.Api

data class Baby(val id: String, val name: String)

class SharedViewModel : ViewModel() {
    private val _spinnerItems = MutableLiveData<List<Api.Companion.Baby>>()
    val spinnerItems: LiveData<List<Api.Companion.Baby>> get() = _spinnerItems

    private lateinit var _deviceId : String
    val deviceId: String get() = _deviceId

    private lateinit var _deviceName : String
    val deviceName: String get() = _deviceName

    private lateinit var _babyId : String
    val babyId: String get() = _babyId

    fun setSpinnerItems(items: List<Api.Companion.Baby>) {
        _spinnerItems.value = items
    }

    fun setDeviceId(deviceId: String) {
        _deviceId = deviceId
    }

    fun setDeviceName(deviceName: String) {
        _deviceName = deviceName
    }

    fun setBabyId(babyId: String) {
        _babyId = babyId
    }
}