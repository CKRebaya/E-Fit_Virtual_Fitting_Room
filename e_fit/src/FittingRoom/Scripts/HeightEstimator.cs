using UnityEngine;
using System.Collections;

public class HeightEstimator : MonoBehaviour 
{
//	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
//	public int playerIndex = 0;

//	[Tooltip("GUI-texture used to display the tracked users on scene background.")]
//	public GUITexture backgroundImage;

	[Tooltip("Smoothing factor used for height estimation.")]
	public float smoothFactor = 5f;

	[Tooltip("UI-Text used to display status messages.")]
	public UnityEngine.UI.Text statusText;

    public UnityEngine.UI.Text sizeText;
    public UnityEngine.UI.Text sizeText2;

    [Tooltip("Estimated user-silhouette height, in meters.")]
	private float userHeight;

	// estimated torso widths
	private float userW1;
	private float userW2;
	private float userW3;
	private float userW4;

//	// user bounds in meters
//	private float userLeft;
//	private float userTop;
//	private float userRight;
//	private float userBottom;

//	// user bounds in depth points
//	private Vector2 posLeft, posTop, posRight, posBottom;

	private KinectManager manager;
	private BodySlicer bodySlicer;
	private long lastFrameTime;


	void Start () 
	{
		manager = KinectManager.Instance;
		bodySlicer = BodySlicer.Instance;

		if (manager && manager.IsInitialized ()) 
		{
//			if(backgroundImage)
//			{
//				Vector3 localScale = backgroundImage.transform.localScale;
//				localScale.x = (float)manager.GetDepthImageWidth() * (float)Screen.height / ((float)manager.GetDepthImageHeight() * (float)Screen.width);
//				localScale.y = -1f;
//
//				backgroundImage.transform.localScale = localScale;
//			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (manager && manager.IsInitialized ()) 
		{
			Texture2D depthImage = manager ? manager.GetUsersLblTex2D() : null;

			if (bodySlicer && bodySlicer.getLastFrameTime() != lastFrameTime) 
			{
				lastFrameTime = bodySlicer.getLastFrameTime();
				int sliceCount = bodySlicer.getBodySliceCount ();

				if (depthImage) 
				{
					//depthImage = GameObject.Instantiate(depthImage) as Texture2D;

					for (int i = 0; i < sliceCount; i++) 
					{
						BodySliceData bodySlice = bodySlicer.getBodySliceData((BodySlice)i);

						if(depthImage && bodySlice.isSliceValid && 
							bodySlice.startDepthPoint != Vector2.zero && bodySlice.endDepthPoint != Vector2.zero)
						{
							KinectInterop.DrawLine(depthImage, (int)bodySlice.startDepthPoint.x, (int)bodySlice.startDepthPoint.y, 
								(int)bodySlice.endDepthPoint.x, (int)bodySlice.endDepthPoint.y, Color.red);
						}
					}

					depthImage.Apply();
				}

				if (statusText) 
				{
                    if (bodySlicer.getCalibratedUserId() != 0)
                    {
                        userHeight = !float.IsNaN(userHeight) ? Mathf.Lerp(userHeight, bodySlicer.getUserHeight(), smoothFactor * Time.deltaTime) : bodySlicer.getUserHeight();
                        string sUserInfo = string.Format("User {0} Height: {1:F2} m", bodySlicer.playerIndex, userHeight);

                        userW1 = !float.IsNaN(userW1) ? Mathf.Lerp(userW1, bodySlicer.getSliceWidth(BodySlice.TORSO_1), smoothFactor * Time.deltaTime) : bodySlicer.getSliceWidth(BodySlice.TORSO_1);
                        userW2 = !float.IsNaN(userW2) ? Mathf.Lerp(userW2, bodySlicer.getSliceWidth(BodySlice.TORSO_2), smoothFactor * Time.deltaTime) : bodySlicer.getSliceWidth(BodySlice.TORSO_2);
                        userW3 = !float.IsNaN(userW3) ? Mathf.Lerp(userW3, bodySlicer.getSliceWidth(BodySlice.TORSO_3), smoothFactor * Time.deltaTime) : bodySlicer.getSliceWidth(BodySlice.TORSO_3);
                        userW4 = !float.IsNaN(userW4) ? Mathf.Lerp(userW4, bodySlicer.getSliceWidth(BodySlice.TORSO_4), smoothFactor * Time.deltaTime) : bodySlicer.getSliceWidth(BodySlice.TORSO_4);

                        sUserInfo += string.Format("\n\nBust: {2:F2} m\nWaist: {1:F2} m\nHips: {0:F2} m", userW1, userW2,  userW4);

                        statusText.text = sUserInfo;

                        string sUserSize;

                        if (userW1 <= 0.39 && userW2 <= 0.31 && userW4 <= 0.38)
                        {
                            sUserSize = ("Your estimated size is: S");
                        }

                        else if (userW1 <= 0.42 && userW2 <= 0.34 && userW4 <= 0.41    )
                        {
                            sUserSize = ("Your estimated size is: M");
                        }

                        else if (userW1 <= 0.45 && userW4 <= 0.44 && userW2 <= 0.39  )
                        {
                            sUserSize = ("Your estimated size is: L");
                        }
                        else
                        {
                            sUserSize = ("Your estimated size is: XL");
                        }

                        //if ((userW4 > 0.345 && userW4 < 0.365) && (userW2 > 0.26 && userW2 < 0.28) && (userW1 >= 0.365 && userW1 < 0.385))
                        //    if ((userW4 <= 0.365 && userW2 <= 0.28 && userW1 <= 0.385) ||
                        //     (userW4 <= 0.365 && userW2 <= 0.31 && userW1 <= 0.385))
                        //        if (userW4 <= 0.400 && userW2 <= 0.30 && userW1 <= 0.400)
                        //        {
                        //            sUserSize = ("Your estimated size is: S");
                        //        }

                        //        else if ((userW4 >= 0.365 || userW4 < 0.395) && (userW2 > 0.28 || userW2 < 0.31) && (userW1 >= 0.385 || userW1 < 0.415))
                        //else if ((userW4 <= 0.395 && userW2 <= 0.31 && userW1 <= 0.415)
                        //        || (userW4 <= 0.400 && userW2 <= 0.33 && userW1 <= 0.415))
                        //        {
                        //            sUserSize = ("Your estimated size is: M");
                        //        }

                        //        else
                        //        {
                        //            sUserSize = ("Your estimated size is: L");
                        //        }

                        sizeText.text = sUserSize;
                        sizeText2.text = sUserSize;

                    } 
                       else
                     {
                          statusText.text = string.Format("User {0} not found", bodySlicer.playerIndex); ;
                     }       

                }

                //if (sizeText)
                //{ 

                //    string sUserSize;

                //    if (userW4 > 0.345 && userW2 > 0.26 && userW1 > 0.365)
                //    {
                //        sUserSize = ("Your size is S");
                //    }

                //    if (userW4 > 0.365 && userW2 > 0.28 && userW1 > 0.385)
                //    {
                //        sUserSize = ("Your size is M");
                //    }

                //    else
                //    {
                //        sUserSize = ("Your size is L");
                //    }

                //    sizeText.text = sUserSize;

                //} 


            }

            //			if (backgroundImage) 
            //			{
            //				backgroundImage.texture = depthImage;
            //			}
        }
	}


}
