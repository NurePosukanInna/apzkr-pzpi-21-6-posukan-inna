package com.example.inventorymobile

import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.UserData
import com.example.inventorymobile.service.ProfileService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class ProfileFragment : Fragment() {

    private lateinit var sharedPreferences: SharedPreferences
    private val connectionClass = ConnectionClass()
    private lateinit var profileService: ProfileService

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_profile, container, false)

        sharedPreferences = requireActivity().getSharedPreferences("UserPreferences", Context.MODE_PRIVATE)
        profileService = ProfileService(connectionClass)

        val userEmail = getUserEmailFromPrefs()

        val emailTextView = view.findViewById<TextView>(R.id.emailTextView)
        val firstNameTextView = view.findViewById<TextView>(R.id.firstNameEditText)
        val lastNameTextView = view.findViewById<TextView>(R.id.lastNameEditText)
        val phoneNumberTextView = view.findViewById<TextView>(R.id.phoneNumberEditText)

        emailTextView.text = userEmail

        fetchProfileDataAndUpdateUI(firstNameTextView, lastNameTextView, phoneNumberTextView)

        val updateButton = view.findViewById<Button>(R.id.updateButton)
        updateButton.setOnClickListener {
            val newFirstName = firstNameTextView.text.toString()
            val newLastName = lastNameTextView.text.toString()
            val newPhoneNumber = phoneNumberTextView.text.toString()
            updateUserDataInBackground(newFirstName, newLastName, newPhoneNumber)
        }

        val logoutButton = view.findViewById<Button>(R.id.logoutButton)
        logoutButton.setOnClickListener {
            logout()
        }

        return view
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        (requireActivity() as? MainActivity)?.setToolbarTitle("Profile")
    }

    private fun getUserEmailFromPrefs(): String {
        return sharedPreferences.getString("email", "") ?: ""
    }

    private fun fetchProfileDataAndUpdateUI(
        firstNameTextView: TextView,
        lastNameTextView: TextView,
        phoneNumberTextView: TextView
    ) {
        lifecycleScope.launch {
            val profileData = withContext(Dispatchers.IO) {
                profileService.fetchProfileData(getUserEmailFromPrefs())
            }
            updateUI(profileData, firstNameTextView, lastNameTextView, phoneNumberTextView)
        }
    }

    private fun updateUI(
        profileData: UserData,
        firstNameTextView: TextView,
        lastNameTextView: TextView,
        phoneNumberTextView: TextView
    ) {
        firstNameTextView.text = profileData.firstName
        lastNameTextView.text = profileData.lastName
        phoneNumberTextView.text = profileData.phoneNumber
    }

    private fun updateUserDataInBackground(newFirstName: String, newLastName: String, newPhoneNumber: String) {
        lifecycleScope.launch {
            val success = withContext(Dispatchers.IO) {
                profileService.updateUserData(getUserEmailFromPrefs(), newFirstName, newLastName, newPhoneNumber)
            }
            if (success) {
                Toast.makeText(context, "Data successfully updated", Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(context, "Failed to update data", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun logout() {
        sharedPreferences.edit().clear().apply()
        val intent = Intent(activity, LoginActivity::class.java)
        startActivity(intent)
        activity?.finish()
    }
}
