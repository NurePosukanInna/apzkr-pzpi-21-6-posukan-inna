package com.example.inventorymobile

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.ListView
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.Data.RequestData
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import java.sql.Connection
import java.sql.SQLException

class RequestFragment : Fragment() {

    private lateinit var requestListView: ListView
    private lateinit var requestAdapter: ArrayAdapter<RequestData>
    private lateinit var connectionClass: ConnectionClass
    private lateinit var sharedPreferences: SharedPreferences

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_request, container, false)

        requestListView = view.findViewById(R.id.listViewRequests)

        requestAdapter = object : ArrayAdapter<RequestData>(
            requireContext(),
            R.layout.item_request,
            R.id.textViewProductName,
            mutableListOf()
        ) {
            override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
                val itemView = convertView ?: LayoutInflater.from(context).inflate(R.layout.item_request, parent, false)
                val requestData = getItem(position)

                val productNameTextView = itemView.findViewById<TextView>(R.id.textViewProductName)
                val quantityTextView = itemView.findViewById<TextView>(R.id.textViewQuantity)
                val requestStatusTextView = itemView.findViewById<TextView>(R.id.textViewRequestStatus)

                productNameTextView.text = requestData?.productName
                quantityTextView.text = "Quantity: ${requestData?.quantity}"
                requestStatusTextView.text = "Status: ${requestData?.requestStatus}"

                return itemView
            }
        }
        requestListView.adapter = requestAdapter

        connectionClass = ConnectionClass()

        sharedPreferences = requireActivity().getSharedPreferences("UserPreferences", Context.MODE_PRIVATE)

        fetchRequests()

        requestListView.setOnItemClickListener { _, _, position, _ ->
            val requestData = requestAdapter.getItem(position)
            requestData?.let {
                navigateToRequestDetail(it.requestId.toString())
            }
        }

        return view
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        (requireActivity() as? MainActivity)?.setToolbarTitle("Request")
    }

    private fun fetchRequests() {
        GlobalScope.launch(Dispatchers.IO) {
            val requests = mutableListOf<RequestData>()
            var connection: Connection? = null
            try {
                connection = connectionClass.connectToSQL()
                if (connection != null) {
                    val query = """
                        SELECT sr.request_id, sr.Quantity, sr.RequestStatus, sr.RequestDate, sr.DeliveryDate, sr.TotalAmount, p.ProductName
                        FROM SupplierRequest sr
                        INNER JOIN store_products sp ON sr.store_product_id = sp.store_product_id
                        INNER JOIN product p ON sp.product_id = p.product_id
                        INNER JOIN store shp ON sp.store_id = shp.store_id
                        WHERE shp.user_id = ?
                    """
                    val preparedStatement = connection.prepareStatement(query)
                    preparedStatement.setInt(1, getUserIdFromPrefs())
                    val resultSet = preparedStatement.executeQuery()

                    while (resultSet.next()) {
                        val requestId = resultSet.getInt("request_id")
                        val productName = resultSet.getString("ProductName")
                        val quantity = resultSet.getInt("Quantity")
                        val requestStatus = resultSet.getString("RequestStatus")
                        val requestDate = resultSet.getDate("RequestDate")
                        val deliveryDate = resultSet.getDate("DeliveryDate")
                        val totalAmount = resultSet.getDouble("TotalAmount")

                        val requestData = RequestData(requestId, productName, quantity, requestStatus, requestDate, deliveryDate, totalAmount)
                        requests.add(requestData)
                    }
                    resultSet.close()
                    preparedStatement.close()
                }
            } catch (e: SQLException) {
                e.printStackTrace()
            } finally {
                try {
                    connection?.close()
                } catch (e: SQLException) {
                    e.printStackTrace()
                }
            }

            launch(Dispatchers.Main) {
                requestAdapter.clear()
                requestAdapter.addAll(requests)
            }
        }
    }

    private fun getUserIdFromPrefs(): Int {
        return sharedPreferences.getInt("user_id", -1)
    }

    private fun navigateToRequestDetail(requestid: String) {
        val fragment = RequestDetailFragment.newInstance(requestid)
        val transaction: FragmentTransaction = parentFragmentManager.beginTransaction()
        transaction.replace(R.id.frame_layout, fragment)
        transaction.addToBackStack(null)
        transaction.commit()
    }
}
