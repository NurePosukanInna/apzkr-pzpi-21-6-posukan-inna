package com.example.inventorymobile.service

import android.content.Context
import android.content.SharedPreferences
import android.os.Build
import android.util.Log
import androidx.annotation.RequiresApi
import com.example.inventorymobile.Connection.ConnectionClass
import java.security.MessageDigest
import java.sql.Connection
import java.sql.ResultSet
import java.util.Base64

class LoginService(private val context: Context, private val connectionClass: ConnectionClass) {

    private var connection: Connection? = null
    private val sharedPreferences: SharedPreferences =
        context.getSharedPreferences("UserPreferences", Context.MODE_PRIVATE)

    @RequiresApi(Build.VERSION_CODES.O)
    fun login(email: String, password: String): Boolean {
        connection = connectionClass.connectToSQL()
        if (connection == null) {
            Log.e("LoginService", "Failed to connect to the database.")
            return false
        }

        val hashedPassword = hashPassword(password)
        Log.d("LoginService", "Hashed Password: $hashedPassword")

        val query = "SELECT * FROM [User] WHERE email = ? AND password = ?"
        return try {
            connection?.prepareStatement(query)?.use { statement ->
                statement.setString(1, email)
                statement.setString(2, hashedPassword)
                Log.d("LoginService", "Executing query: $query with email: $email and hashedPassword: $hashedPassword")
                val rs: ResultSet = statement.executeQuery()
                val loginSuccess = rs.next()
                if (loginSuccess) {
                    saveEmailToPrefs(email)
                    Log.d("LoginService", "Login successful for email: $email")
                } else {
                    Log.d("LoginService", "Login failed for email: $email")
                }
                loginSuccess
            } ?: false
        } catch (e: Exception) {
            Log.e("LoginService", "Error logging in: ${e.message}", e)
            false
        } finally {
            connection?.close()
        }
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private fun hashPassword(password: String): String {
        return try {
            val messageDigest = MessageDigest.getInstance("SHA-256")
            val hashedBytes = messageDigest.digest(password.toByteArray())
            Base64.getEncoder().encodeToString(hashedBytes)
        } catch (e: Exception) {
            Log.e("LoginService", "Error hashing password: ${e.message}", e)
            ""
        }
    }

    private fun saveEmailToPrefs(email: String) {
        with(sharedPreferences.edit()) {
            putString("email", email)
            apply()
        }
    }

    fun getEmailFromPrefs(): String {
        return sharedPreferences.getString("email", "") ?: ""
    }
}
