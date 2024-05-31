package com.example.babymonitorapplication

import TokenManager
import android.content.ContentValues.TAG
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.activity.viewModels
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.ContextCompat
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.example.babymonitorapplication.databinding.ActivityMainBinding
import com.example.babymonitorapplication.ui.register.SharedViewModel
import com.google.android.gms.tasks.OnCompleteListener
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.google.firebase.messaging.FirebaseMessaging
import com.journeyapps.barcodescanner.ScanContract
import com.journeyapps.barcodescanner.ScanIntentResult
import com.journeyapps.barcodescanner.ScanOptions


class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding
    private lateinit var name: EditText
    private lateinit var username: EditText
    private lateinit var email: EditText
    private lateinit var password: EditText
    private lateinit var confirmPassword: EditText
    private lateinit var signupBtn: Button

    private lateinit var authEmail: EditText
    private lateinit var authPassword: EditText
    private lateinit var signinBtn: Button

    private lateinit var api : Api

    private lateinit var deviceId : String
    private lateinit var spinner: Spinner

    private val sharedViewModel: SharedViewModel by viewModels()

    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission(),
    ) { isGranted: Boolean ->
        if (isGranted) {
            // FCM SDK (and your app) can post notifications.
        } else {
            // TODO: Inform user that that your app will not show notifications.
        }
    }

    private fun askNotificationPermission() {
        // This is only necessary for API level >= 33 (TIRAMISU)
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
            if (ContextCompat.checkSelfPermission(this, android.Manifest.permission.POST_NOTIFICATIONS) ==
                PackageManager.PERMISSION_GRANTED
            ) {
                // FCM SDK (and your app) can post notifications.
            } else if (shouldShowRequestPermissionRationale(android.Manifest.permission.POST_NOTIFICATIONS)) {
                // TODO: display an educational UI explaining to the user the features that will be enabled
                //       by them granting the POST_NOTIFICATION permission. This UI should provide the user
                //       "OK" and "No thanks" buttons. If the user selects "OK," directly request the permission.
                //       If the user selects "No thanks," allow the user to continue without notifications.
            } else {
                // Directly ask for the permission
                requestPermissionLauncher.launch(android.Manifest.permission.POST_NOTIFICATIONS)
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val navView: BottomNavigationView = binding.navView

        val navController = findNavController(R.id.nav_host_fragment_activity_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        val appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.navigation_signup, R.id.navigation_signin
            )
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        val tokenManager = TokenManager(this)
        val cookie = tokenManager.getToken()

        if (cookie != null) {
            showSecondView()
        }

        askNotificationPermission()

        FirebaseMessaging.getInstance().getToken()
            .addOnCompleteListener(OnCompleteListener<String?> { task ->
                if (!task.isSuccessful) {
                    Log.w(TAG, "Fetching FCM registration token failed", task.exception)
                    return@OnCompleteListener
                }

                // Get new FCM registration token
                val token = task.result

                // Log and toast
                Log.d(TAG, "FCM Registration Token: $token")
                println("Yoken: " + token)
            })
    }

    private fun showSecondView() {
        val navView: BottomNavigationView = findViewById(R.id.nav_view)
        navView.visibility = View.GONE // Hide the bottom navigation view

        val navController = findNavController(R.id.nav_host_fragment_activity_main)
        val appBarConfiguration = AppBarConfiguration(setOf(R.id.navigation_main))
        setupActionBarWithNavController(navController, appBarConfiguration)

        // Navigate to the main fragment
        navController.navigate(R.id.navigation_main)
    }

    public fun signupClick(view: View?) {
        // get text fields
        name = findViewById(R.id.name)
        username = findViewById(R.id.username)
        email = findViewById(R.id.email)
        password = findViewById(R.id.password)
        confirmPassword = findViewById(R.id.confirmPassword)

        val nameText = name.text.toString()
        val usernameText = username.text.toString()
        val emailText = email.text.toString()
        val passwordText = password.text.toString()
        val confirmPasswordText = confirmPassword.text.toString()

        if (passwordText == confirmPasswordText) {
            Api.signup("users/create", nameText, usernameText, emailText, passwordText) { success ->
                if (success) {
                    runOnUiThread {
                        name.text = null
                        username.text = null
                        email.text = null
                        password.text = null
                        confirmPassword.text = null

                        findNavController(R.id.nav_host_fragment_activity_main).navigate(R.id.navigation_signin)
                        findNavController(R.id.nav_host_fragment_activity_main).popBackStack()
                    }
                } else {
                    println("Not success")
                }
            }
        }
    }

    public fun signinClick(view: View?) {
        authEmail = findViewById(R.id.authEmail)
        authPassword = findViewById(R.id.authPassword)

        val emailText = authEmail.text.toString()
        val passwordText = authPassword.text.toString()
        Api.loginAndGetCookies("users/authenticate", emailText, passwordText, this) { success ->
            if (success) {
                runOnUiThread {
                    authEmail.text = null
                    authPassword.text = null
                    showSecondView()
                }
            } else {
                println("Not success")
            }
        }
    }

    public fun logout(view: View?) {
        Api.logout("users/logout",this) { success ->
            if (success) {
                println("logged out")
                runOnUiThread {
                    val navView: BottomNavigationView = findViewById(R.id.nav_view)
                    navView.visibility = View.VISIBLE // Hide the bottom navigation view
                    val navController = findNavController(R.id.nav_host_fragment_activity_main)
                    navController.popBackStack()
                    navController.navigate(R.id.navigation_signin)

                    val tokenManager = TokenManager(this)
                    tokenManager.clearToken()
                }
            } else {
                println("Not success")
            }
            runOnUiThread {
                val navView: BottomNavigationView = findViewById(R.id.nav_view)
                navView.visibility = View.VISIBLE // Hide the bottom navigation view
                val navController = findNavController(R.id.nav_host_fragment_activity_main)
                navController.popBackStack()
                navController.navigate(R.id.navigation_signin)

                val tokenManager = TokenManager(this)
                tokenManager.clearToken()
            }
        }
    }

    private fun initiateRegister(deviceId: String) {
        println("Device id: " + deviceId)

        Api.getBabies("baby/all", this) { success, babies ->
            if (success) {
                println("success")
                babies.forEach {
                    println(it.name)
                }
                runOnUiThread {
                    sharedViewModel.setSpinnerItems(babies)
                }
            } else {
                println("not success")
            }
        }

        sharedViewModel.setDeviceId(deviceId)
    }

    public fun requestRegisterDevice(view: View?) {
        Api.registerDevice("device/register", this, sharedViewModel.deviceId, sharedViewModel.deviceName, sharedViewModel.babyId) {success ->
            if (success) {
                println("success")
            } else {
                println("not success")
            }
            runOnUiThread {
                findNavController(R.id.nav_host_fragment_activity_main).navigate(R.id.navigation_main)
            }
        }
    }

    private val barcodeLauncher = registerForActivityResult<ScanOptions, ScanIntentResult>(
        ScanContract()
    ) { result: ScanIntentResult ->
        if (result.contents == null) {
            println("canceled")
        } else {
            initiateRegister(result.contents)
            println("Scanned: " + result.contents)
            val navController = findNavController(R.id.nav_host_fragment_activity_main)
            navController.navigate(R.id.navigation_register)
            deviceId = result.contents
        }
    }

    public fun registerDevice(view: View?) {
        val options = ScanOptions()
        options.setOrientationLocked(false);
        options.setBarcodeImageEnabled(true);
        options.setPrompt("Scan the QR code on your device");
        barcodeLauncher.launch(options)
    }
}