package com.example.inventorymobile

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.ListView
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.service.ProductService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class ProductFragment : Fragment() {

    private lateinit var storeId: String
    private lateinit var productListView: ListView
    private lateinit var productAdapter: ArrayAdapter<Triple<String, String, Int>>
    private lateinit var productService: ProductService
    private var products = mutableListOf<Triple<String, String, Int>>()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        storeId = arguments?.getString("store_id") ?: ""
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_product, container, false)
        productListView = view.findViewById(R.id.listViewProducts)

        productAdapter = object : ArrayAdapter<Triple<String, String, Int>>(
            requireContext(),
            R.layout.item_product,
            R.id.textViewProductName,
            mutableListOf()
        ) {
            override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
                val view = convertView ?: LayoutInflater.from(context).inflate(R.layout.item_product, parent, false)
                val productNameTextView = view.findViewById<TextView>(R.id.textViewProductName)
                val categoryTextView = view.findViewById<TextView>(R.id.textViewCategory)
                val (productName, categoryName, _) = getItem(position)!!
                productNameTextView.text = productName
                categoryTextView.text = "Category: $categoryName"
                return view
            }
        }
        productListView.adapter = productAdapter

        productListView.onItemClickListener = AdapterView.OnItemClickListener { _, _, position, _ ->
            val selectedProduct = products[position]
            val productId = selectedProduct.third

            val productDetailFragment = ProductDetailFragment.newInstance(productId)
            parentFragmentManager.beginTransaction()
                .replace(R.id.frame_layout, productDetailFragment)
                .addToBackStack(null)
                .commit()
        }

        productService = ProductService(requireContext(), ConnectionClass())

        lifecycleScope.launch {
            val fetchedProducts = withContext(Dispatchers.IO) {
                productService.fetchProducts(storeId)
            }
            updateUI(fetchedProducts)
        }

        return view
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        (requireActivity() as? MainActivity)?.setToolbarTitle("Products")
    }

    private fun updateUI(fetchedProducts: List<Triple<String, String, Int>>) {
        products.clear()
        products.addAll(fetchedProducts)
        productAdapter.clear()
        if (fetchedProducts.isNotEmpty()) {
            productAdapter.addAll(fetchedProducts)
        } else {
            productAdapter.add(Triple("No products found", "", 0))
        }
    }

    companion object {
        @JvmStatic
        fun newInstance(storeId: String) = ProductFragment().apply {
            arguments = Bundle().apply {
                putString("store_id", storeId)
            }
        }
    }
}
