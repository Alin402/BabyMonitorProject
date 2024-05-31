package com.example.babymonitorapplication.ui.register

import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import android.widget.Toast
import androidx.core.widget.doOnTextChanged
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import com.example.babymonitorapplication.R

class RegisterFragment() : Fragment(R.layout.fragment_register) {
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        return super.onCreateView(inflater, container, savedInstanceState)
    }
    private val sharedViewModel: SharedViewModel by activityViewModels()
    private lateinit var deviceName : EditText

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Find the spinner
        val spinner: Spinner = view.findViewById(R.id.spinner)
        val deviceName: EditText = view.findViewById(R.id.deviceName)
        this.deviceName = deviceName
        val saveBtn : Button = view.findViewById(R.id.saveBtn)

        sharedViewModel.spinnerItems.observe(viewLifecycleOwner) { items ->
            // Map Baby names to display in the spinner
            val babyNames: List<String> = items.map { it.name }

            // Create a custom adapter that displays baby names but retains baby IDs as the underlying data
            val adapter = object : ArrayAdapter<String>(
                requireContext(),
                android.R.layout.simple_spinner_item,
                babyNames
            ) {
                override fun getItem(position: Int): String? {
                    // Return the baby name
                    return babyNames[position]
                }

                override fun getItemId(position: Int): Long {
                    // Return the corresponding baby ID as a string representation of the UUID
                    return position.toLong()
                }

                override fun hasStableIds(): Boolean {
                    // Set to true to ensure proper selection tracking
                    return true
                }
            }

            // Set the adapter for the spinner
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            spinner.adapter = adapter
        }

        spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                // Retrieve the selected ID from the adapter using the position
                val selectedId: String = sharedViewModel.spinnerItems.value?.get(position)?.id ?: ""

                // Do something with the selected ID
                sharedViewModel.setBabyId(selectedId)
            }

            override fun onNothingSelected(parent: AdapterView<*>?) {
                // Handle case when nothing is selected
            }
        }

        deviceName.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {
            }

            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {
            }

            override fun afterTextChanged(s: Editable?) {
                val text = s.toString()
                sharedViewModel.setDeviceName(text)
            }
        })
    }
}