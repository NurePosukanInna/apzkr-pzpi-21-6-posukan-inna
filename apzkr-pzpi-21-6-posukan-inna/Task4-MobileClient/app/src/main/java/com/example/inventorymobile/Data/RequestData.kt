package com.example.inventorymobile.Data

import java.util.Date

data class RequestData(
    val requestId: Int,
    val productName: String,
    val quantity: Int,
    val requestStatus: String,
    val requestDate: Date?,
    val deliveryDate: Date?,
    val totalAmount: Double
)
