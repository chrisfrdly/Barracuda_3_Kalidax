using UnityEngine;

[CreateAssetMenu(fileName = "Current Quota Data", menuName = "Data Containers/Current Quota")]
public class SO_Data_CurrentQuota : ScriptableObject
{
    public int[] quotaArray;

    public void Initialize(int size)
    {
        quotaArray = new int[size];
        for (int i = 0; i < quotaArray.Length; i++)
        {
            quotaArray[i] = 0; // Initialize all quotas to 0
        }
    }
}
