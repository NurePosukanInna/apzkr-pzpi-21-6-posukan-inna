package com.example.inventorymobile.Data

data class ProductData(
    var productName: String? = null,
    var productQuantity: Int? = null,
    var productMinQuantity: Int? = null,
    var productVolume: Double? = null,
    var productMeasureOfUnits: String? = null,
    var productPrice: Double? = null
)
