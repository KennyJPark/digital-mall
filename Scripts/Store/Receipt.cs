using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Generated and returned by ItemSaleController.ConfirmSaleAsync()
 * to be used by BalanceSheet.cs
 * */

//public class Receipt : MonoBehaviour
public class Receipt
{
    private List<ItemForSale> _itemsSold;
    private int _numItemsSold;
    private int _revenue;

    public Receipt()
    {
        _numItemsSold = 0;
        _revenue = 0;
    }

    public Receipt(int numItemsSold, int rev)
    {
        _numItemsSold = numItemsSold;
        _revenue = rev;
    }

    public void PrintReceipt()
    {
        Debug.Log("Printing Receipt:\n");
        Debug.Log("_numItemsSold:" + _numItemsSold);
        Debug.Log("_revenue:" + _revenue);
    }

    public int GetRevenue()
    {
        return _revenue;
    }

    public int GetNumItemsSold()
    {
        return _numItemsSold;
    }

    public List<ItemForSale> GetItemsSold()
    {
        return _itemsSold;
    }

    public void SetRevenue(int rev)
    {
        _revenue = rev;
    }

    public void SetNumItemsSold(int numItemsSold)
    {
        _numItemsSold = numItemsSold;
    }

    public void AddItemToList(ItemForSale soldItem)
    {
        _itemsSold.Add(soldItem);
    }

}
