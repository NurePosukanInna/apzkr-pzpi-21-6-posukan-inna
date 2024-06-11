package com.example.inventorymobile.service

import android.content.Context
import com.example.inventorymobile.Connection.ConnectionClass
import java.sql.Connection
import java.sql.ResultSet
import java.sql.SQLException

class ProductService(private val context: Context, private val connectionClass: ConnectionClass) {

    fun fetchProducts(storeId: String): List<Triple<String, String, Int>> {
        val products = mutableListOf<Triple<String, String, Int>>()
        val connection: Connection? = connectionClass.connectToSQL()
        try {
            if (connection != null) {
                val query = """
                    SELECT p.product_id, p.ProductName, c.CategoryName
                    FROM product p 
                    JOIN Store_Products sp ON p.product_id = sp.product_id 
                    JOIN category c ON p.category_id = c.category_id
                    WHERE sp.store_id = ?
                """
                val preparedStatement = connection.prepareStatement(query)
                preparedStatement.setString(1, storeId)
                val resultSet: ResultSet = preparedStatement.executeQuery()
                while (resultSet.next()) {
                    val productId = resultSet.getInt("product_id")
                    val productName = resultSet.getString("ProductName")
                    val categoryName = resultSet.getString("CategoryName")
                    products.add(Triple(productName, categoryName, productId))
                }
                resultSet.close()
                preparedStatement.close()
            }
        } catch (e: SQLException) {
            e.printStackTrace()
        } finally {
            connection?.close()
        }
        return products
    }
}
