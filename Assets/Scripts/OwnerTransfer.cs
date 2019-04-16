using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerTransfer : Photon.PunBehaviour
{
    private void OnMouseDown()
    {
        base.photonView.RequestOwnership();
    }

    public override void OnOwnershipRequest(object[] viewAndPlayer)
    {
        PhotonView view = viewAndPlayer[0] as PhotonView;
        PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
        base.photonView.TransferOwnership(requestingPlayer);
    }
}
