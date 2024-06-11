package com.example.inventorymobile.Connection

import android.annotation.SuppressLint
import android.os.StrictMode
import android.os.StrictMode.ThreadPolicy
import android.util.Log
import java.sql.Connection
import java.sql.DriverManager

class ConnectionClass {
    var con: Connection? = null

    @SuppressLint("NewApi")
    fun connectToSQL(): Connection? {
        val ip = "192.168.0.100"
        val port = "1433"
        val db = "Inventory"
        val username = "dbAdmin"
        val password = "Csergo228"
        val policy = ThreadPolicy.Builder().permitAll().build()
        StrictMode.setThreadPolicy(policy)
        return try {
            Class.forName("net.sourceforge.jtds.jdbc.Driver")
            val connectUrl =
                "jdbc:jtds:sqlserver://$ip:$port;databaseName=$db;user=$username;password=$password;"
            con = DriverManager.getConnection(connectUrl)
            if (con != null) {
                Log.d("ConSql", "Database connected successfully")
                con
            } else {
                Log.d("ConSql", "Failed to connect to the database")
                null
            }
        } catch (e: Exception) {
            Log.e("ConSql", "Error: " + e.message)
            null
        }
    }
}
