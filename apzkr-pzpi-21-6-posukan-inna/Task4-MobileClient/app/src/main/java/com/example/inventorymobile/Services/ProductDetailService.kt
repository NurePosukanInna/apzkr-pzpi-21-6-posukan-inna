package com.example.inventorymobile.service

import android.content.Context
import android.util.Log
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.ProductData
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.sql.Connection
import java.sql.SQLException

class ProductDetailService(private val context: Context, private val connectionClass: ConnectionClass) {
    suspend fun fetchProductDetail(productId: Int): ProductData {
        return withContext(Dispatchers.IO) {
            val productData = ProductData()
            var connection: Connection? = null
            try {
                connection = connectionClass.connectToSQL()
                if (connection != null) {
                    val query = """
                    SELECT ProductName, quantity, MinQuantity, Price, Volume, MeasureOfUnits
                    FROM product
                    JOIN Store_Products ON product.product_id = Store_Products.product_id
                    WHERE product.product_id = ?
                """
                    val preparedStatement = connection.prepareStatement(query)
                    preparedStatement.setInt(1, productId)
                    val resultSet = preparedStatement.executeQuery()
                    if (resultSet.next()) {
                        productData.productName = resultSet.getString("ProductName")
                        productData.productQuantity = resultSet.getInt("quantity")
                        productData.productMinQuantity = resultSet.getInt("MinQuantity")
                        productData.productVolume = resultSet.getDouble("Volume")
                        productData.productMeasureOfUnits = resultSet.getString("MeasureOfUnits")
                        productData.productPrice = resultSet.getDouble("Price")
                    }

                    resultSet.close()
                    preparedStatement.close()
                }
            } catch (e: SQLException) {
                Log.e("ProductService", "Error fetching product details: ${e.message}", e)
            } finally {
                connection?.close()
            }
            productData
        }
    }

    suspend fun updateProductDetails(productId: Int, productData: ProductData): Boolean {
        return withContext(Dispatchers.IO) {
            var success = false
            var connection: Connection? = null
            try {
                connection = connectionClass.connectToSQL()
                if (connection != null) {
                    connection.autoCommit = false

                    val updateProductQuery = """
                    UPDATE product
                    SET ProductName = ?,
                        Price = ?,
                        Volume = ?,
                        MeasureOfUnits = ?
                    WHERE product_id = ?
                """
                    val updateProductStatement = connection.prepareStatement(updateProductQuery)
                    updateProductStatement.setString(1, productData.productName)
                    updateProductStatement.setDouble(2, productData.productPrice ?: 0.0)
                    updateProductStatement.setDouble(3, productData.productVolume ?: 0.0)
                    updateProductStatement.setString(4, productData.productMeasureOfUnits)
                    updateProductStatement.setInt(5, productId)
                    val rowsAffectedProduct = updateProductStatement.executeUpdate()

                    val updateStoreProductsQuery = """
                    UPDATE Store_Products
                    SET quantity = ?,
                        MinQuantity = ?
                    WHERE product_id = ?
                """
                    val updateStoreProductsStatement =
                        connection.prepareStatement(updateStoreProductsQuery)
                    updateStoreProductsStatement.setInt(1, productData.productQuantity ?: 0)
                    updateStoreProductsStatement.setInt(2, productData.productMinQuantity ?: 0)
                    updateStoreProductsStatement.setInt(3, productId)
                    val rowsAffectedStoreProducts = updateStoreProductsStatement.executeUpdate()

                    success = rowsAffectedProduct > 0 && rowsAffectedStoreProducts > 0

                    if (success) {
                        connection.commit()
                    } else {
                        connection.rollback()
                    }

                    updateProductStatement.close()
                    updateStoreProductsStatement.close()
                }
            } catch (e: SQLException) {
                Log.e("ProductService", "Error updating product details: ${e.message}", e)
                if (connection != null) {
                    try {
                        connection.rollback()
                    } catch (ex: SQLException) {
                        Log.e("ProductService", "Error rolling back transaction: ${ex.message}", ex)
                    }
                }
            } finally {
                try {
                    connection?.autoCommit = true
                    connection?.close()
                } catch (ex: SQLException) {
                    Log.e("ProductService", "Error closing connection: ${ex.message}", ex)
                }
            }
            success
        }
    }
}
