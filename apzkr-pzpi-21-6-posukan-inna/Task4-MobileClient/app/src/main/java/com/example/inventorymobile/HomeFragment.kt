package com.example.inventorymobile

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.ListView
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.StoreData
import com.example.inventorymobile.service.HomeService
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class HomeFragment : Fragment() {
    private lateinit var connectionClass: ConnectionClass
    private lateinit var databaseService: HomeService
    private lateinit var listViewStores: ListView
    private lateinit var storeAdapter: ArrayAdapter<StoreData>
    private val storeIds = mutableListOf<String>()
    private lateinit var sharedPreferences: SharedPreferences

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_home, container, false)

        val activity = requireActivity() as MainActivity
        activity.setToolbarTitle("Your Shop")

        listViewStores = view.findViewById(R.id.listViewStores)
        connectionClass = ConnectionClass()
        databaseService = HomeService(requireContext(), connectionClass)
        sharedPreferences = requireContext().getSharedPreferences("UserPreferences", Context.MODE_PRIVATE)

        storeAdapter = object : ArrayAdapter<StoreData>(requireContext(), R.layout.item_shop, mutableListOf()) {
            override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
                val itemView = convertView ?: LayoutInflater.from(context).inflate(R.layout.item_shop, parent, false)
                val storeData = getItem(position)

                val storeNameTextView = itemView.findViewById<TextView>(R.id.textViewStoreName)
                val temperatureTextView = itemView.findViewById<TextView>(R.id.textViewTemperature)
                val humidityTextView = itemView.findViewById<TextView>(R.id.textViewHumidity)

                storeNameTextView.text = storeData?.storeName
                temperatureTextView.text = "Temperature: ${storeData?.temperature}°C"
                humidityTextView.text = "Humidity: ${storeData?.humidity}%"

                return itemView
            }
        }
        listViewStores.adapter = storeAdapter

        listViewStores.onItemClickListener = AdapterView.OnItemClickListener { _, _, position, _ ->
            val storeId = storeIds[position]
            navigateToProduct(storeId)
        }

        fetchStores()

        return view
    }

    private fun navigateToProduct(storeId: String) {
        val fragment = ProductFragment.newInstance(storeId)
        val transaction: FragmentTransaction = parentFragmentManager.beginTransaction()
        transaction.replace(R.id.frame_layout, fragment)
        transaction.addToBackStack(null)
        transaction.commit()
    }

    private fun saveUserIdToPrefs(userId: Int) {
        sharedPreferences.edit().putInt("user_id", userId).apply()
    }

    private fun fetchStores() {
        val email = sharedPreferences.getString("email", "") ?: ""
        CoroutineScope(Dispatchers.IO).launch {
            val userId = databaseService.getUserIdFromEmail(email)
            val storesWithSensors = databaseService.fetchStores(userId)
            withContext(Dispatchers.Main) {
                updateUI(storesWithSensors)
                saveUserIdToPrefs(userId)
            }
        }
    }

    private fun updateUI(storesWithSensors: List<StoreData>) {
        storeAdapter.clear()
        storeIds.clear()
        if (storesWithSensors.isNotEmpty()) {
            storesWithSensors.forEach { storeData ->
                storeAdapter.add(storeData)
                storeIds.add(storeData.storeId)
            }
        } else {
            Toast.makeText(requireContext(), "No stores found for user", Toast.LENGTH_LONG).show()
        }
        Log.i("User ID from SharedPreferences", sharedPreferences.getInt("user_id", -1).toString())
    }
}
