package com.example.inventorymobile

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.inventorymobile.Connection.ConnectionClass
import com.example.inventorymobile.service.RequestDetailService
import java.sql.SQLException
import java.text.SimpleDateFormat
import java.util.*

class RequestDetailFragment : Fragment() {
    lateinit var productNameTextView: TextView
    lateinit var quantityEditText: EditText
    lateinit var requestStatusSpinner: Spinner
    lateinit var requestDateTextView: TextView
    lateinit var deliveryDateTextView: TextView
    lateinit var totalAmountTextView: TextView
    private lateinit var sharedPreferences: SharedPreferences
    private lateinit var connectionClass: ConnectionClass

    var isDeliveryDateUpdated = false
    private val requestDetailService by lazy { RequestDetailService(requireContext()) }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_request_detail, container, false)

        productNameTextView = view.findViewById(R.id.textViewProductName)
        quantityEditText = view.findViewById(R.id.editTextQuantity)
        requestStatusSpinner = view.findViewById(R.id.spinnerRequestStatus)
        requestDateTextView = view.findViewById(R.id.textViewRequestDate)
        deliveryDateTextView = view.findViewById(R.id.textViewDeliveryDate)
        totalAmountTextView = view.findViewById(R.id.textViewTotalAmount)

        sharedPreferences = requireActivity().getSharedPreferences("UserPreferences", Context.MODE_PRIVATE)
        connectionClass = ConnectionClass()

        val requestId = arguments?.getString(ARG_REQUEST_ID)

        loadDataFromDatabase(requestId)

        val updateButton: Button = view.findViewById(R.id.buttonUpdateRequest)
        updateButton.setOnClickListener {
            updateDataInDatabase(requestId)
        }

        return view
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        (requireActivity() as? MainActivity)?.setToolbarTitle("Request Details")
    }
    private fun loadDataFromDatabase(requestId: String?) {
        requestDetailService.loadDataFromDatabase(requestId, this)
    }

    private fun updateDataInDatabase(requestId: String?) {
        requestDetailService.updateDataInDatabase(requestId, this)
    }

    private fun getUserIdFromPrefs(): Int {
        return sharedPreferences.getInt("user_id", -1)
    }

    companion object {
        private const val ARG_REQUEST_ID = "request_id"

        @JvmStatic
        fun newInstance(requestId: String): RequestDetailFragment {
            val fragment = RequestDetailFragment()
            val args = Bundle()
            args.putString(ARG_REQUEST_ID, requestId)
            fragment.arguments = args
            return fragment
        }
    }
}
