package crc64c44d4aef22441422;


public class MainActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		android.view.SurfaceHolder.Callback2,
		android.view.SurfaceHolder.Callback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_surfaceRedrawNeeded:(Landroid/view/SurfaceHolder;)V:GetSurfaceRedrawNeeded_Landroid_view_SurfaceHolder_Handler:Android.Views.ISurfaceHolderCallback2Invoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceRedrawNeededAsync:(Landroid/view/SurfaceHolder;Ljava/lang/Runnable;)V:GetSurfaceRedrawNeededAsync_Landroid_view_SurfaceHolder_Ljava_lang_Runnable_Handler:Android.Views.ISurfaceHolderCallback2, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceChanged:(Landroid/view/SurfaceHolder;III)V:GetSurfaceChanged_Landroid_view_SurfaceHolder_IIIHandler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceCreated:(Landroid/view/SurfaceHolder;)V:GetSurfaceCreated_Landroid_view_SurfaceHolder_Handler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_surfaceDestroyed:(Landroid/view/SurfaceHolder;)V:GetSurfaceDestroyed_Landroid_view_SurfaceHolder_Handler:Android.Views.ISurfaceHolderCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("community_canvas_sk_v1.MainActivity, community_canvas_sk_v1_Android", MainActivity.class, __md_methods);
	}


	public MainActivity ()
	{
		super ();
		if (getClass () == MainActivity.class) {
			mono.android.TypeManager.Activate ("community_canvas_sk_v1.MainActivity, community_canvas_sk_v1_Android", "", this, new java.lang.Object[] {  });
		}
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


	public void surfaceRedrawNeeded (android.view.SurfaceHolder p0)
	{
		n_surfaceRedrawNeeded (p0);
	}

	private native void n_surfaceRedrawNeeded (android.view.SurfaceHolder p0);


	public void surfaceRedrawNeededAsync (android.view.SurfaceHolder p0, java.lang.Runnable p1)
	{
		n_surfaceRedrawNeededAsync (p0, p1);
	}

	private native void n_surfaceRedrawNeededAsync (android.view.SurfaceHolder p0, java.lang.Runnable p1);


	public void surfaceChanged (android.view.SurfaceHolder p0, int p1, int p2, int p3)
	{
		n_surfaceChanged (p0, p1, p2, p3);
	}

	private native void n_surfaceChanged (android.view.SurfaceHolder p0, int p1, int p2, int p3);


	public void surfaceCreated (android.view.SurfaceHolder p0)
	{
		n_surfaceCreated (p0);
	}

	private native void n_surfaceCreated (android.view.SurfaceHolder p0);


	public void surfaceDestroyed (android.view.SurfaceHolder p0)
	{
		n_surfaceDestroyed (p0);
	}

	private native void n_surfaceDestroyed (android.view.SurfaceHolder p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
