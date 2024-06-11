package com.example.inventorymobile.service

import android.util.Log
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.UserData
import java.sql.Connection
import java.sql.SQLException

class ProfileService(private val connectionClass: ConnectionClass) {

    fun fetchProfileData(userEmail: String): UserData {
        var profileData = UserData("Undefined", "Undefined", "Undefined")
        var connection: Connection? = null
        try {
            connection = connectionClass.connectToSQL()
            if (connection != null) {
                val query = "SELECT FirstName, LastName, PhoneNumber FROM [User] WHERE email = ?"
                connection.prepareStatement(query).use { preparedStatement ->
                    preparedStatement.setString(1, userEmail)
                    val resultSet = preparedStatement.executeQuery()
                    if (resultSet.next()) {
                        val firstName = resultSet.getString("FirstName") ?: "Undefined"
                        val lastName = resultSet.getString("LastName") ?: "Undefined"
                        val phoneNumber = resultSet.getString("PhoneNumber") ?: "Undefined"
                        profileData = UserData(firstName, lastName, phoneNumber)
                    }
                }
            }
        } catch (e: SQLException) {
            Log.e("ProfileService", "Error fetching profile data: ${e.message}", e)
        } finally {
            connection?.close()
        }
        return profileData
    }

    fun updateUserData(userEmail: String, newFirstName: String, newLastName: String, newPhoneNumber: String): Boolean {
        var connection: Connection? = null
        return try {
            connection = connectionClass.connectToSQL()
            if (connection != null) {
                val query = "UPDATE [User] SET FirstName = ?, LastName = ?, PhoneNumber = ? WHERE email = ?"
                connection.prepareStatement(query).use { preparedStatement ->
                    preparedStatement.setString(1, newFirstName)
                    preparedStatement.setString(2, newLastName)
                    preparedStatement.setString(3, newPhoneNumber)
                    preparedStatement.setString(4, userEmail)
                    preparedStatement.executeUpdate()
                }
                true
            } else {
                false
            }
        } catch (e: SQLException) {
            Log.e("ProfileService", "Error updating user data: ${e.message}", e)
            false
        } finally {
            connection?.close()
        }
    }
}
