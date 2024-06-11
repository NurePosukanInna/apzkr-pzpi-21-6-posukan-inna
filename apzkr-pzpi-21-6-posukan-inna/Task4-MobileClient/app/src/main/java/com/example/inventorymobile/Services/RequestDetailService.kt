package com.example.inventorymobile.service

import android.content.Context
import android.widget.ArrayAdapter
import android.widget.Toast
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.RequestDetailFragment
import java.sql.SQLException
import java.text.SimpleDateFormat
import java.util.*

class RequestDetailService(private val context: Context) {

    private val connectionClass = ConnectionClass()

    fun loadDataFromDatabase(requestId: String?, fragment: RequestDetailFragment) {
        val query = """
            SELECT sr.request_id, sr.Quantity, sr.RequestStatus, sr.RequestDate, sr.DeliveryDate, sr.TotalAmount, p.ProductName
            FROM SupplierRequest sr
            INNER JOIN store_products sp ON sr.store_product_id = sp.store_product_id
            INNER JOIN product p ON sp.product_id = p.product_id
            WHERE sr.request_id = '$requestId'
        """

        try {
            val connection = connectionClass.connectToSQL()
            val preparedStatement = connection?.prepareStatement(query)
            val resultSet = preparedStatement?.executeQuery()

            if (resultSet != null && resultSet.next()) {
                fragment.productNameTextView.text = resultSet.getString("ProductName")
                fragment.quantityEditText.setText(resultSet.getInt("Quantity").toString())

                val requestDate = resultSet.getDate("RequestDate")
                fragment.requestDateTextView.text = requestDate?.toString() ?: ""

                val deliveryDate = resultSet.getDate("DeliveryDate")
                fragment.deliveryDateTextView.text = deliveryDate?.toString() ?: "undefined"

                fragment.totalAmountTextView.text = resultSet.getDouble("TotalAmount").toString()

                val statusArray = arrayOf("Pending", "Completed")
                val adapter = ArrayAdapter(context, android.R.layout.simple_spinner_item, statusArray)
                adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                fragment.requestStatusSpinner.adapter = adapter

                val requestStatus = resultSet.getString("RequestStatus")
                if (requestStatus != null) {
                    val statusPosition = if (requestStatus == "Pending") 0 else 1
                    fragment.requestStatusSpinner.setSelection(statusPosition)
                }
            }

            resultSet?.close()
            preparedStatement?.close()
            connection?.close()
        } catch (e: SQLException) {
            e.printStackTrace()
        }
    }

    fun updateDataInDatabase(requestId: String?, fragment: RequestDetailFragment) {
        val newStatus = fragment.requestStatusSpinner.selectedItem.toString()
        val newQuantity = fragment.quantityEditText.text.toString().toInt()

        try {
            val connection = connectionClass.connectToSQL()
            val preparedStatement = connection?.prepareStatement("UPDATE SupplierRequest SET Quantity = ?, RequestStatus = ? WHERE request_id = ?")
            preparedStatement?.setInt(1, newQuantity)
            preparedStatement?.setString(2, newStatus)
            preparedStatement?.setString(3, requestId)
            preparedStatement?.executeUpdate()

            // Only update the delivery date if the status is "Completed" and it hasn't been updated yet
            if (newStatus == "Completed" && !fragment.isDeliveryDateUpdated) {
                val currentDate = Calendar.getInstance().time
                val dateFormat = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())
                val formattedDate = dateFormat.format(currentDate)
                val deliveryDate = java.sql.Date.valueOf(formattedDate)

                val deliveryDateStatement = connection?.prepareStatement("UPDATE SupplierRequest SET DeliveryDate = ? WHERE request_id = ?")
                deliveryDateStatement?.setDate(1, deliveryDate)
                deliveryDateStatement?.setString(2, requestId)
                deliveryDateStatement?.executeUpdate()

                fragment.isDeliveryDateUpdated = true
            }

            preparedStatement?.close()
            connection?.close()

            // Refresh the data displayed in the fragment
            loadDataFromDatabase(requestId, fragment)
        } catch (e: SQLException) {
            e.printStackTrace()
        }
    }
}
