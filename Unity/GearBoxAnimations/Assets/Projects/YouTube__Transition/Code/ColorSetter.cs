using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorSetter : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;
    
    [SerializeField]
    private Color m_BaseColor;
    
    [SerializeField]
    private float m_Blend;
    
    [SerializeField]
    private bool m_Refresh;
    
    void Update()
    {
        if (m_Refresh)
        {
            m_Refresh = false;

            m_Image.color = Color.Lerp(m_BaseColor, Random.ColorHSV(), m_Blend);
        }
    }
}
