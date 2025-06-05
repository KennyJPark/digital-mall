using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaseable
{
    void Purchase();

    void ProcessPrice();

    int ProcessOffer(int offer);
    //int ProcessOffer();
}
