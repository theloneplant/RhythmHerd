using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdGatherer : MonoBehaviour
{
    [SerializeField] private Herd herd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        HerdMember member = other.gameObject.GetComponent<HerdMember>();
        if (member != null && member.GetState() == HerdMember.MemberState.Roam && !member.IsDisoriented())
        {
            Debug.Log("is member, adding to list");
            herd.AddMember(member);
            member.setState(HerdMember.MemberState.Rejoin);
        }
    }
}
