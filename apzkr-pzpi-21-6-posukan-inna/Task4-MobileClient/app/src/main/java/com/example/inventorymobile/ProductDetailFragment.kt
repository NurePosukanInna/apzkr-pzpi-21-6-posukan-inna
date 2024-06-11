// ProductDetailFragment.kt
package com.example.inventorymobile

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.ProductData
import com.example.inventorymobile.service.ProductDetailService
import kotlinx.coroutines.launch

class ProductDetailFragment : Fragment() {

    private var productId: Int? = null
    private lateinit var productService: ProductDetailService
    private lateinit var productNameEditText: EditText
    private lateinit var productQuantityEditText: EditText
    private lateinit var productMinQuantityEditText: EditText
    private lateinit var productPriceEditText: EditText
    private lateinit var productVolumeEditText: EditText
    private lateinit var productMeasureOfUnitsEditText: EditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            productId = it.getInt("product_id")
        }
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        (requireActivity() as? MainActivity)?.setToolbarTitle("Product Details")
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_product_detail, container, false)
        productNameEditText = view.findViewById(R.id.editTextProductName)
        productQuantityEditText = view.findViewById(R.id.editTextProductQuantity)
        productMinQuantityEditText = view.findViewById(R.id.editTextProductMinQuantity)
        productPriceEditText = view.findViewById(R.id.editTextProductPrice)
        productVolumeEditText = view.findViewById(R.id.editTextProductVolume)
        productMeasureOfUnitsEditText = view.findViewById(R.id.editTextProductMeasureOfUnits)

        productService = ProductDetailService(requireContext(), ConnectionClass())

        lifecycleScope.launch {
            productId?.let { id ->
                val productData = productService.fetchProductDetail(id)
                updateUI(productData)
            } ?: run {
                Toast.makeText(requireContext(), "Invalid product ID", Toast.LENGTH_SHORT).show()
            }
        }

        view.findViewById<Button>(R.id.buttonUpdateProduct).setOnClickListener {
            lifecycleScope.launch {
                productId?.let { id ->
                    val productData = collectProductData()
                    val success = productService.updateProductDetails(id, productData)
                    if (success) {
                        Toast.makeText(
                            requireContext(),
                            "Product details updated successfully",
                            Toast.LENGTH_SHORT
                        ).show()
                    } else {
                        Toast.makeText(
                            requireContext(),
                            "Failed to update product details",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                } ?: run {
                    Toast.makeText(requireContext(), "Invalid product ID", Toast.LENGTH_SHORT).show()
                }
            }
        }

        return view
    }

    private fun updateUI(productData: ProductData) {
        productNameEditText.setText(productData.productName ?: "")
        productQuantityEditText.setText(productData.productQuantity?.toString() ?: "")
        productMinQuantityEditText.setText(productData.productMinQuantity?.toString() ?: "")
        productVolumeEditText.setText(productData.productVolume?.toString() ?: "")
        productMeasureOfUnitsEditText.setText(productData.productMeasureOfUnits ?: "")
        productPriceEditText.setText(productData.productPrice?.toString() ?: "")
    }

    private fun collectProductData(): ProductData {
        return ProductData(
            productName = productNameEditText.text.toString(),
            productQuantity = productQuantityEditText.text.toString().toIntOrNull(),
            productMinQuantity = productMinQuantityEditText.text.toString().toIntOrNull(),
            productVolume = productVolumeEditText.text.toString().toDoubleOrNull(),
            productMeasureOfUnits = productMeasureOfUnitsEditText.text.toString(),
            productPrice = productPriceEditText.text.toString().toDoubleOrNull()
        )
    }

    companion object {
        @JvmStatic
        fun newInstance(productId: Int) =
            ProductDetailFragment().apply {
                arguments = Bundle().apply {
                    putInt("product_id", productId)
                }
            }
    }
}
