
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

//
// System.Reflection.Emit/UnmanagedMarshal.cs
//
// Author:
//   Paolo Molaro (lupus@ximian.com)
//
// (C) 2001-2002 Ximian, Inc.  http://www.ximian.com
//

using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System;

namespace System.Reflection.Emit {

	[Serializable]
	public sealed class UnmanagedMarshal {
		private int count;
		private UnmanagedType t;
		private UnmanagedType tbase;
		string guid;
		string mcookie;
		string marshaltype;
		Type marshaltyperef;
		
		private UnmanagedMarshal (UnmanagedType maint, int cnt) {
			count = cnt;
			t = maint;
			tbase = maint;
		}
		private UnmanagedMarshal (UnmanagedType maint, UnmanagedType elemt) {
			count = 0;
			t = maint;
			tbase = elemt;
		}
		
		public UnmanagedType BaseType {
			get {
				if (t == UnmanagedType.LPArray || t == UnmanagedType.SafeArray)
					throw new ArgumentException ();
				return tbase;
			}
		}

		public int ElementCount {
			get {return count;}
		}

		public UnmanagedType GetUnmanagedType {
			get {return t;}
		}

		public Guid IIDGuid {
			get {return new Guid (guid);}
		}

		public static UnmanagedMarshal DefineByValArray( int elemCount) {
			return new UnmanagedMarshal (UnmanagedType.ByValArray, elemCount);
		}

		public static UnmanagedMarshal DefineByValTStr( int elemCount) {
			return new UnmanagedMarshal (UnmanagedType.ByValTStr, elemCount);
		}

		public static UnmanagedMarshal DefineLPArray( UnmanagedType elemType) {
			return new UnmanagedMarshal (UnmanagedType.LPArray, elemType);
		}

		public static UnmanagedMarshal DefineSafeArray( UnmanagedType elemType) {
			return new UnmanagedMarshal (UnmanagedType.SafeArray, elemType);
		}

		public static UnmanagedMarshal DefineUnmanagedMarshal( UnmanagedType unmanagedType) {
			return new UnmanagedMarshal (unmanagedType, unmanagedType);
		}

		/* this one is missing from the MS implementation */
		public static UnmanagedMarshal DefineCustom (Type typeref, string cookie, string mtype, Guid id) {
			UnmanagedMarshal res = new UnmanagedMarshal (UnmanagedType.CustomMarshaler, UnmanagedType.CustomMarshaler);
			res.mcookie = cookie;
			res.marshaltype = mtype;
			res.marshaltyperef = typeref;
			if (id == Guid.Empty)
				res.guid = "";
			else
				res.guid = id.ToString ();
			return res;
		}

		internal MarshalAsAttribute ToMarshalAsAttribute () {
			MarshalAsAttribute attr = new MarshalAsAttribute (t);
			attr.ArraySubType = tbase;
			attr.MarshalCookie = mcookie;
			attr.MarshalType = marshaltype;
			attr.MarshalTypeRef = marshaltyperef;
			attr.SizeConst = count;
			return attr;
		}
	}
}
