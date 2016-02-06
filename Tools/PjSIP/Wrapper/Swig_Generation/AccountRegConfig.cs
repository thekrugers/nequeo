//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.7
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class AccountRegConfig : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal AccountRegConfig(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(AccountRegConfig obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~AccountRegConfig() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pjsua2PINVOKE.delete_AccountRegConfig(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public SWIGTYPE_p_string registrarUri {
    set {
      pjsua2PINVOKE.AccountRegConfig_registrarUri_set(swigCPtr, SWIGTYPE_p_string.getCPtr(value));
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      SWIGTYPE_p_string ret = new SWIGTYPE_p_string(pjsua2PINVOKE.AccountRegConfig_registrarUri_get(swigCPtr), true);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public bool registerOnAdd {
    set {
      pjsua2PINVOKE.AccountRegConfig_registerOnAdd_set(swigCPtr, value);
    } 
    get {
      bool ret = pjsua2PINVOKE.AccountRegConfig_registerOnAdd_get(swigCPtr);
      return ret;
    } 
  }

  public SWIGTYPE_p_SipHeaderVector headers {
    set {
      pjsua2PINVOKE.AccountRegConfig_headers_set(swigCPtr, SWIGTYPE_p_SipHeaderVector.getCPtr(value));
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      SWIGTYPE_p_SipHeaderVector ret = new SWIGTYPE_p_SipHeaderVector(pjsua2PINVOKE.AccountRegConfig_headers_get(swigCPtr), true);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public uint timeoutSec {
    set {
      pjsua2PINVOKE.AccountRegConfig_timeoutSec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_timeoutSec_get(swigCPtr);
      return ret;
    } 
  }

  public uint retryIntervalSec {
    set {
      pjsua2PINVOKE.AccountRegConfig_retryIntervalSec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_retryIntervalSec_get(swigCPtr);
      return ret;
    } 
  }

  public uint firstRetryIntervalSec {
    set {
      pjsua2PINVOKE.AccountRegConfig_firstRetryIntervalSec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_firstRetryIntervalSec_get(swigCPtr);
      return ret;
    } 
  }

  public uint randomRetryIntervalSec {
    set {
      pjsua2PINVOKE.AccountRegConfig_randomRetryIntervalSec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_randomRetryIntervalSec_get(swigCPtr);
      return ret;
    } 
  }

  public uint delayBeforeRefreshSec {
    set {
      pjsua2PINVOKE.AccountRegConfig_delayBeforeRefreshSec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_delayBeforeRefreshSec_get(swigCPtr);
      return ret;
    } 
  }

  public bool dropCallsOnFail {
    set {
      pjsua2PINVOKE.AccountRegConfig_dropCallsOnFail_set(swigCPtr, value);
    } 
    get {
      bool ret = pjsua2PINVOKE.AccountRegConfig_dropCallsOnFail_get(swigCPtr);
      return ret;
    } 
  }

  public uint unregWaitMsec {
    set {
      pjsua2PINVOKE.AccountRegConfig_unregWaitMsec_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_unregWaitMsec_get(swigCPtr);
      return ret;
    } 
  }

  public uint proxyUse {
    set {
      pjsua2PINVOKE.AccountRegConfig_proxyUse_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.AccountRegConfig_proxyUse_get(swigCPtr);
      return ret;
    } 
  }

  public virtual void readObject(SWIGTYPE_p_ContainerNode node) {
    pjsua2PINVOKE.AccountRegConfig_readObject(swigCPtr, SWIGTYPE_p_ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void writeObject(SWIGTYPE_p_ContainerNode node) {
    pjsua2PINVOKE.AccountRegConfig_writeObject(swigCPtr, SWIGTYPE_p_ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

  public AccountRegConfig() : this(pjsua2PINVOKE.new_AccountRegConfig(), true) {
  }

}