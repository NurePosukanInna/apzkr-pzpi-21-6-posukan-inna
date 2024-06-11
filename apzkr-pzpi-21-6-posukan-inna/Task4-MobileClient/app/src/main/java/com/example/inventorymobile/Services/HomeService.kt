package com.example.inventorymobile.service

import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.os.Build
import android.util.Log
import androidx.core.app.NotificationCompat
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.StoreData
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import java.sql.Connection
import java.sql.SQLException

class HomeService(private val context: Context, private val connectionClass: ConnectionClass) {

    private val CHANNEL_ID = "TemperatureAlert"

    fun getUserIdFromEmail(email: String): Int {
        var userId = -1
        val connection: Connection? = connectionClass.connectToSQL()
        try {
            if (connection != null) {
                val query = "SELECT user_id FROM [User] WHERE email = ?"
                val preparedStatement = connection.prepareStatement(query)
                preparedStatement.setString(1, email)
                val resultSet = preparedStatement.executeQuery()
                if (resultSet.next()) {
                    userId = resultSet.getInt("user_id")
                }
                resultSet.close()
                preparedStatement.close()
            }
        } catch (e: SQLException) {
            Log.e("DatabaseService", "Error getting user ID from email: ${e.message}", e)
        } finally {
            connection?.close()
        }
        return userId
    }

    fun fetchStores(userId: Int): List<StoreData> {
        val storesWithSensors = mutableListOf<StoreData>()
        val connection: Connection? = connectionClass.connectToSQL()
        try {
            if (connection != null) {
                val query = """
                    SELECT s.store_id, s.StoreName, se.temperature, se.humidity 
                    FROM store s 
                    JOIN sensor se ON s.store_id = se.store_id 
                    WHERE s.user_id = ?
                """
                val preparedStatement = connection.prepareStatement(query)
                preparedStatement.setInt(1, userId)
                val resultSet = preparedStatement.executeQuery()
                while (resultSet.next()) {
                    val storeId = resultSet.getString("store_id")
                    val storeName = resultSet.getString("StoreName")
                    val temperature = resultSet.getDouble("temperature")
                    val humidity = resultSet.getDouble("humidity")
                    storesWithSensors.add(StoreData(storeId, storeName, temperature, humidity))

                    if (temperature <= 17 || temperature > 22) {
                        GlobalScope.launch(Dispatchers.Main) {
                            sendNotification(storeName, "Temperature", temperature)
                        }
                    }

                    if (humidity < 40 || humidity > 60) {
                        GlobalScope.launch(Dispatchers.Main) {
                            sendNotification(storeName, "Humidity", humidity)
                        }
                    }
                }
                resultSet.close()
                preparedStatement.close()
            }
        } catch (e: SQLException) {
            Log.e("DatabaseService", "Error fetching stores: ${e.message}", e)
        } finally {
            connection?.close()
        }
        return storesWithSensors
    }

    private suspend fun sendNotification(storeName: String, parameter: String, value: Double) {
        val notificationManager =
            context.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                CHANNEL_ID,
                "Parameter Alert",
                NotificationManager.IMPORTANCE_DEFAULT
            )
            notificationManager.createNotificationChannel(channel)
        }

        val builder = NotificationCompat.Builder(context, CHANNEL_ID)
            .setSmallIcon(android.R.drawable.ic_dialog_alert)
            .setContentTitle("$parameter Alert")
            .setContentText("$storeName $parameter is $value.")
            .setPriority(NotificationCompat.PRIORITY_DEFAULT)

        notificationManager.notify(0, builder.build())
    }
}
