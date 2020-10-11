/* ************************************************* *
/* DataTools Visual Basic Utility Library 
/*
/* Module: Internal CIL Code For MemPtr Structure
/* 
/* Copyright (C) 2011-2020 Nathan Moschkin
/* All Rights Reserved
/*
/* Licensed Under the Microsoft Public License   
/* ************************************************* */

.namespace DataTools.Memory
{
    .class public MemPtr
    {


        // **** STATIC MEMBERS *** //



        /* END STATIC MEMBERS */


		
        
		// **** int8/uint8 value type arrays
        

		// operator MemPtr to uint8 array
        .method public specialname static uint8[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			
            call       instance uint8[] DataTools.Memory.MemPtr::GetByteArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to uint8 array
        .method public instance uint8[] GetByteArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                uint8[] x
            )

            ldarg.s     length
            newarr      uint8
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     uint8
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator uint8 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(uint8[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetByteArray(native int, uint8[])
            ldloc.s      0
            ret

        }
        
		// function uint8 array to MemPtr
		.method public instance void SetByteArray(native int byteIndex, uint8[] values) cil managed
        {
              .maxstack 4
              .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8 
            stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     uint8

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }




        

		// operator MemPtr to int8 array
        .method public specialname static int8[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			
            call       instance int8[] DataTools.Memory.MemPtr::GetSByteArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to int8 array
        .method public instance int8[] GetSByteArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                int8[] x
            )

            ldarg.s     length
            newarr      [System.Runtime]System.SByte
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     [System.Runtime]System.SByte
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator int8 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(int8[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetSByteArray(native int, int8[])
            ldloc.s      0
            ret

        }
        
		// function int8 array to MemPtr
		.method public instance void SetSByteArray(native int byteIndex, int8[] values) cil managed
        {
            
              .maxstack 4
              .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8 
            stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     [System.Runtime]System.SByte

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }
















        
		// operator MemPtr to char array
        .method public specialname static char[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.1
			shr

            call       instance char[] DataTools.Memory.MemPtr::GetCharArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to char array
        .method public instance char[] GetCharArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                char[] x
            )

            ldarg.s     length
            newarr      char
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     char
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.1
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator char array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(char[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetCharArray(native int, char[])
            ldloc.s      0
            ret

        }
        
		// function char array to MemPtr
		.method public instance void SetCharArray(native int byteIndex, char[] values) cil managed
        {
            .maxstack   4
            .locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8
            
            ldc.i4.1
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     char

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }

























        
		// **** int16/uint16 value type arrays
        

		// operator MemPtr to uint16 array
        .method public specialname static uint16[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.1
			shr

            call       instance uint16[] DataTools.Memory.MemPtr::GetUShortArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to uint16 array
        .method public instance uint16[] GetUShortArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                uint16[] x
            )

            ldarg.s     length
            newarr      uint16
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     uint16
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.1
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator uint16 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(uint16[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetUShortArray(native int, uint16[])
            ldloc.s      0
            ret

        }
        
		// function uint16 array to MemPtr
		.method public instance void SetUShortArray(native int byteIndex, uint16[] values) cil managed
        {
            .maxstack   4
            .locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            
            conv.i8
            ldc.i4.1
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     uint16

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }









        

		// operator MemPtr to int16 array
        .method public specialname static int16[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.1
			shr

            call       instance int16[] DataTools.Memory.MemPtr::GetShortArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to int16 array
        .method public instance int16[] GetShortArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                int16[] x
            )

            ldarg.s     length
            newarr      int16
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     int16
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.1
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator int16 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(int16[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetShortArray(native int, int16[])
            ldloc.s      0
            ret

        }
        
		// function int16 array to MemPtr
		.method public instance void SetShortArray(native int byteIndex, int16[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.1
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     int16

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }






























        
		// **** int32/uint32 value type arrays
        

		// operator MemPtr to uint32 array
        .method public specialname static uint32[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.2
			shr

            call       instance uint32[] DataTools.Memory.MemPtr::GetUIntegerArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to uint32 array
        .method public instance uint32[] GetUIntegerArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                uint32[] x
            )

            ldarg.s     length
            newarr      uint32
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     uint32
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.2
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator uint32 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(uint32[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetUIntegerArray(native int, uint32[])
            ldloc.s      0
            ret

        }
        
		// function uint32 array to MemPtr
		.method public instance void SetUIntegerArray(native int byteIndex, uint32[] values) cil managed
        {
            .maxstack 4
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.2
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     uint32

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }




        

		// operator MemPtr to int32 array
        .method public specialname static int32[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.2
			shr

            call       instance int32[] DataTools.Memory.MemPtr::GetIntegerArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to int32 array
        .method public instance int32[] GetIntegerArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                int32[] x
            )

            ldarg.s     length
            newarr      int32
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     int32
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.2
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator int32 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(int32[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetIntegerArray(native int, int32[])
            ldloc.s      0
            ret

        }
        
		// function int32 array to MemPtr
		.method public instance void SetIntegerArray(native int byteIndex, int32[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
                int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.2
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)

            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     int32

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }















		// **** int64/uint64 value type arrays
        
 
		// operator MemPtr to int64 array
        .method public specialname static uint64[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.3
			shr

            call       instance uint64[] DataTools.Memory.MemPtr::GetULongArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to int64 array
        .method public instance uint64[] GetULongArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                uint64[] x
            )

            ldarg.s     length
            newarr      uint64
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     uint64
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.3
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator int64 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(uint64[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetULongArray(native int, uint64[])
            ldloc.s      0
            ret

        }
        
		// function int64 array to MemPtr
		.method public instance void SetULongArray(native int byteIndex, uint64[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.3
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     uint64

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }












 
		// operator MemPtr to int64 array
        .method public specialname static int64[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.3
			shr

            call       instance int64[] DataTools.Memory.MemPtr::GetLongArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to int64 array
        .method public instance int64[] GetLongArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                int64[] x
            )

            ldarg.s     length
            newarr      int64
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     int64
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.3
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator int64 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(int64[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetLongArray(native int, int64[])
            ldloc.s      0
            ret

        }
        
		// function int64 array to MemPtr
		.method public instance void SetLongArray(native int byteIndex, int64[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.3
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     int64

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }



		
		// **** float value type arrays


        

		// operator MemPtr to float32 array
        .method public specialname static float32[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.2
			shr

            call       instance float32[] DataTools.Memory.MemPtr::GetSingleArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to float32 array
        .method public instance float32[] GetSingleArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                float32[] x
            )

            ldarg.s     length
            newarr      float32
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     float32
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.2
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator float32 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(float32[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetSingleArray(native int, float32[])
            ldloc.s      0
            ret

        }
        
		// function float32 array to MemPtr
		.method public instance void SetSingleArray(native int byteIndex, float32[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.2
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     float32

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }




		// operator MemPtr to float64 array
        .method public specialname static float64[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.3
			shr

            call       instance float64[] DataTools.Memory.MemPtr::GetDoubleArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to float64 array
        .method public instance float64[] GetDoubleArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                float64[] x
            )

            ldarg.s     length
            newarr      float64
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     float64
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.3
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator float64 array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(float64[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetDoubleArray(native int, float64[])
            ldloc.s      0
            ret

        }
        
		// function float64 array to MemPtr
		.method public instance void SetDoubleArray(native int byteIndex, float64[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.3
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     float64

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }








		
		// **** decimal/guid value type arrays
		


		

		// operator MemPtr to valuetype [System.Runtime]System.Decimal array
        .method public specialname static valuetype [System.Runtime]System.Decimal[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.4
			shr

            call       instance valuetype [System.Runtime]System.Decimal[] DataTools.Memory.MemPtr::GetDecimalArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to valuetype [System.Runtime]System.Decimal array
        .method public instance valuetype [System.Runtime]System.Decimal[] GetDecimalArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                valuetype [System.Runtime]System.Decimal[] x
            )

            ldarg.s     length
            newarr      [System.Runtime]System.Decimal
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     [System.Runtime]System.Decimal
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.4
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator valuetype [System.Runtime]System.Decimal array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(valuetype [System.Runtime]System.Decimal[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetDecimalArray(native int, valuetype [System.Runtime]System.Decimal[])
            ldloc.s      0
            ret

        }
        
		// function valuetype [System.Runtime]System.Decimal array to MemPtr
		.method public instance void SetDecimalArray(native int byteIndex, valuetype [System.Runtime]System.Decimal[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.4
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     [System.Runtime]System.Decimal

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }




		// **** [System.Runtime]System.Guid ****


		
		
		

		// operator MemPtr to valuetype [System.Runtime]System.Decimal array
        .method public specialname static valuetype [System.Runtime]System.Guid[] op_Implicit(valuetype DataTools.Memory.MemPtr operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 8
			.locals init
            (
                valuetype [System.Runtime]System.Nullable`1<native int> nuln,
				int64 lenIn
            )
            
            ldarga.s   0
            ldloc.s    nuln

            call       instance int64 DataTools.Memory.MemPtr::Length([opt] valuetype [System.Runtime]System.Nullable`1<native int>)

			stloc.s    lenIn
			ldloc.s    lenIn
			ldc.i8     0
			ble  NULRET

        
			ldarga.s   0

		    ldc.i4.0
			conv.i
		
        	ldloc.s    lenIn
			conv.i4
			ldc.i4.4
			shr

            call       instance valuetype [System.Runtime]System.Guid[] DataTools.Memory.MemPtr::GetGuidArray(native int, int32)
        
			br		   RET
		NULRET:
			ldc.i4.0
			conv.i
		
		RET:
            ret
        }
    	
		
		// function MemPtr to valuetype [System.Runtime]System.Guid array
        .method public instance valuetype [System.Runtime]System.Guid[] GetGuidArray(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 8
            .locals init
            (
                valuetype [System.Runtime]System.Guid[] x
            )

            ldarg.s     length
            newarr      [System.Runtime]System.Guid
            stloc.s     x
            ldloc.s     x
            ldc.i4.0
            ldelema     [System.Runtime]System.Guid
            
            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     length
			ldc.i4.4
			shl
			conv.u4
			            
            cpblk
            
            ldloc.s     x
            ret		
        		
        }

		
		// operator valuetype [System.Runtime]System.Guid array to MemPtr
        .method public specialname static valuetype DataTools.Memory.MemPtr op_Implicit(valuetype [System.Runtime]System.Guid[] operand) cil managed 
        {
			// THIS IS GOOD
            .maxstack 3
            .locals init
            (
                valuetype DataTools.Memory.MemPtr mm
            )

            ldloca.s     0
            ldc.i4.0
			conv.i
            ldarg.s 0
            call instance void DataTools.Memory.MemPtr::SetGuidArray(native int, valuetype [System.Runtime]System.Guid[])
            ldloc.s      0
            ret

        }
        
		// function valuetype [System.Runtime]System.Guid array to MemPtr
		.method public instance void SetGuidArray(native int byteIndex, valuetype [System.Runtime]System.Guid[] values) cil managed
        {
            .maxstack 4            
            .locals init
            (
			    int64 size
            )

            ldarg.s     values
            ldc.i4.0
            beq RET

            ldarg.s     values
            call		instance int32 [System.Runtime]System.Array::get_Length()
            conv.i8

            ldc.i4.4
			shl
			stloc.s     size
                              
            ldarg.s 0
            ldind.i
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg.s     byteIndex

            ldarg.s 0
            ldloc.s     size
            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:

            ldarg.s 0
            ldind.i
            ldarg.s     byteIndex
            add

            ldarg.s     values
            ldc.i4.0
            ldelema     [System.Runtime]System.Guid

            ldloc.s     size
            conv.u4
                               
            cpblk

    RET:
            ret		
        }







        // **** Singleton value types ****




        
        //
        //
        // uint8 Byte , int8 SByte.
        //
        //
        
        .method public specialname instance void set_ByteAt(int64 byteIndex, uint8 v) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add

            ldarg.s 2
            stind.i1
            ret

        }

        .method public specialname instance uint8 get_ByteAt(int64 byteIndex) cil managed
        {
            .maxstack 3
            

			ldarg.s 0
            ldind.i

	        ldarg.s 1
            conv.i
            add

			ldind.u1
			ret
            
        }

                
        .method public specialname instance void set_SByteAt(int64 byteIndex, int8 v) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add

            ldarg.s 2
            stind.i1
            ret

        }

        .method public specialname instance int8 get_SByteAt(int64 byteIndex) cil managed
        {
            .maxstack 3
            

			ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add

			ldind.i1
			ret
            
        }

        
        .method public specialname instance void set_CharAt(int64 index, char v) cil managed
        {
            .maxstack 3

            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.1
            shl
    
            conv.i
        
            add

            ldarg.s 2
			stind.i2
			ret

       
        }




        // char (uint16)

        .method public specialname instance char get_CharAt(int64 index) cil managed
        {
			.maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.1
            shl
    
            conv.i
        
            add

			ldind.u2
			ret
        }

        .method public specialname instance void set_CharAtAbsolute(int64 byteIndex, char v) cil managed
        {
           .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add

            ldarg.s 2
            stind.i2

            ret
        }

        .method public specialname instance char get_CharAtAbsolute(int64 byteIndex) cil managed
        {
           .maxstack 3
           
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add

            ldind.u2

            ret
        }




        //
        //
        // 16 bit integers.
        //
        //

        // signed 16-bit
        
        .method public specialname instance void set_ShortAt(int64 index, int16 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.1
            shl

            conv.i

            add
            
            ldarg.s 2
            stind.i2

            ret
        }

        .method public specialname instance int16 get_ShortAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1

            ldc.i4.1
            shl

            conv.i

            add
            ldind.i2
            ret
        }

        .method public specialname instance void set_ShortAtAbsolute(int64 byteIndex, int16 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i2

            ret
        }

        .method public specialname instance int16 get_ShortAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.i2
            ret
        }


        // unsigned 16-bit 


        .method public specialname instance void set_UShortAt(int64 index, uint16 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.1
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.i2

            ret
        }

        .method public specialname instance uint16 get_UShortAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.1
            shl
            conv.i

            add
            ldind.u2
            ret
        }

        .method public specialname instance void set_UShortAtAbsolute(int64 byteIndex, uint16 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i2

            ret
        }

        .method public specialname instance uint16 get_UShortAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.u2
            ret
        }



    

        //
        //
        // 32 bit integers.
        //
        //

        // signed 32-bit
        
        .method public specialname instance void set_IntegerAt(int64 index, int32 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.i4

            ret
        }

        .method public specialname instance int32 get_IntegerAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            ldind.i4
            ret
        }

        .method public specialname instance void set_IntegerAtAbsolute(int64 byteIndex, int32 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i4

            ret
        }

        .method public specialname instance int32 get_IntegerAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.i4
            ret
        }


        // unsigned 32-bit 


        .method public specialname instance void set_UIntegerAt(int64 index, uint32 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.i4

            ret
        }

        .method public specialname instance uint32 get_UIntegerAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            ldind.u4
            ret
        }

        .method public specialname instance void set_UIntegerAtAbsolute(int64 byteIndex, uint32 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i4

            ret
        }

        .method public specialname instance uint32 get_UIntegerAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.u4
            ret
        }



        //
        //
        // 64 bit integers.
        //
        //

        // signed 64-bit
        
        .method public specialname instance void set_LongAt(int64 index, int64 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.i8

            ret
        }

        .method public specialname instance int64 get_LongAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            ldind.i8
            ret
        }

        .method public specialname instance void set_LongAtAbsolute(int64 byteIndex, int64 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i8

            ret
        }

        .method public specialname instance int64 get_LongAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.i8
            ret
        }


        // unsigned 64-bit 


        .method public specialname instance void set_ULongAt(int64 index, uint64 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.i8

            ret
        }

        .method public specialname instance uint64 get_ULongAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            ldind.u8
            ret
        }

        .method public specialname instance void set_ULongAtAbsolute(int64 byteIndex, uint64 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.i8

            ret
        }

        .method public specialname instance uint64 get_ULongAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.u8
            ret
        }






        //
        //
        // float 32 and float 64.
        //
        //

        .method public specialname instance void set_SingleAt(int64 index, float32 v) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.r4

            ret
        }

        .method public specialname instance float32 get_SingleAt(int64 index) cil managed
        {
            .maxstack 3
            
            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.2
            shl
            conv.i

            add
            ldind.r4
            ret
        }

        .method public specialname instance void set_SingleAtAbsolute(int64 byteIndex, float32 v) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.r4

            ret
        }

        .method public specialname instance float32 get_SingleAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.r4
            ret
        }





        .method public specialname instance void set_DoubleAt(int64 index, float64 v) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            
            ldarg.s 2
            stind.r8

            ret
        }

        .method public specialname instance float64 get_DoubleAt(int64 index) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            ldc.i4.3
            shl
            conv.i

            add
            ldind.r8
            ret
        }

        .method public specialname instance void set_DoubleAtAbsolute(int64 byteIndex, float64 v) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i
           
            ldarg.s 1
            conv.i
            add
            
            ldarg.s 2
            stind.r8

            ret
        }

        .method public specialname instance float64 get_DoubleAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            

            ldarg.s 0
            ldind.i

            ldarg.s 1
            conv.i
            add
           
            ldind.r8
            ret
        }




		
        .method public specialname instance void set_DecimalAt(int64 byteIndex, valuetype [System.Runtime]System.Decimal v) cil managed
        {
           .maxstack 3

            // we will multiply instead of shifting so that we can easily work with negative values.
            

            ldarg.s 1
            ldc.i4.4
            shl
            conv.i
            ldarg.s 0
            ldind.i
            add

            ldarga.s 2

            ldc.i4.s 16
            conv.u4

            cpblk
        
            ret
        }

        .method public specialname instance valuetype [System.Runtime]System.Decimal get_DecimalAt(int64 byteIndex) cil managed
        {
            .maxstack 3
            .locals init 
            (
                
                valuetype [System.Runtime]System.Decimal x
            )
            

            ldloca 0

            ldarg.s 1
            ldc.i4.4
            shl
            conv.i
            ldarg.s 0
            ldind.i
            add

            ldc.i4.s 16
            conv.u4

            cpblk
        
            ldloc.0
            ret
        }

        .method public specialname instance void set_DecimalAtAbsolute(int64 byteIndex, valuetype [System.Runtime]System.Decimal v) cil managed
        {
           .maxstack 3
            

            ldarg.s 0
            ldind.i
            ldarg.s 1
            conv.i        
            add

            ldarga.s 2
            ldc.i4.s 16
            conv.u4

            cpblk
           
            ret
        }

        .method public specialname instance valuetype [System.Runtime]System.Decimal get_DecimalAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            .locals init 
            (
                valuetype [System.Runtime]System.Decimal x
            )
            

            ldloca 0

            ldarg.s 0
            ldind.i
            ldarg.s 1
            conv.i
            add
            
            ldc.i4.s 16
            conv.u4

            cpblk
        
            ldloc.0
            ret
        }




		// **** Guid indexers



		
        .method public specialname instance void set_GuidAt(int64 byteIndex, valuetype [System.Runtime]System.Guid v) cil managed
        {
           .maxstack 3

            // we will multiply instead of shifting so that we can easily work with negative values.
            

            ldarg.s 1
            ldc.i4.4
            shl
            conv.i
            ldarg.s 0
            ldind.i
            add

            ldarga.s 2

            ldc.i4.s 16
            conv.u4

            cpblk
        
            ret
        }

        .method public specialname instance valuetype [System.Runtime]System.Guid get_GuidAt(int64 byteIndex) cil managed
        {
            .maxstack 3
            .locals init 
            (
                
                valuetype [System.Runtime]System.Guid x
            )
            

            ldloca 0

            ldarg.s 1
            ldc.i4.4
            shl
            conv.i
            ldarg.s 0
            ldind.i
            add

            ldc.i4.s 16
            conv.u4

            cpblk
        
            ldloc.0
            ret
        }

        .method public specialname instance void set_GuidAtAbsolute(int64 byteIndex, valuetype [System.Runtime]System.Guid v) cil managed
        {
           .maxstack 3
            

            ldarg.s 0
            ldind.i
            ldarg.s 1
            conv.i        
            add

            ldarga 2
            ldc.i4.s 16
            conv.u4

            cpblk
           
            ret
        }

        .method public specialname instance valuetype [System.Runtime]System.Guid get_GuidAtAbsolute(int64 byteIndex) cil managed
        {
            .maxstack 3
            .locals init 
            (
                valuetype [System.Runtime]System.Guid x
            )
            

            ldloca 0

            ldarg.s 0
            ldind.i
            ldarg.s 1
            conv.i
            add
            
            ldc.i4.s 16
            conv.u4

            cpblk
        
            ldloc.0
            ret
        }


        
		// Byte and SByte arrays

		// DEPRECATED
        .method public instance uint8[] GrabBytes(native int byteIndex, int32 length) cil managed 
        {
            jmp        instance uint8[] DataTools.Memory.MemPtr::GetByteArray(native int, int32)
        }

        .method public instance void SetBytes(native int byteIndex, uint8[] bytes) cil managed
        {
            jmp        instance void DataTools.Memory.MemPtr::SetByteArray(native int, uint8[])
        }
        
        .method public instance int8[] GrabSBytes(native int byteIndex, int32 length) cil managed 
        {
            jmp        instance int8[] DataTools.Memory.MemPtr::GetSByteArray(native int, int32)
        }

        .method public instance void SetSBytes(native int byteIndex, int8[] bytes) cil managed
        {
            jmp        instance void DataTools.Memory.MemPtr::SetSByteArray(native int, int8[])
        }
        
        /////////////////////////////////////////////////
        //                                             //
        //  Functions to grab/set strings.             //
        //                                             //
        /////////////////////////////////////////////////





		// function string to MemPtr

        
        
        // function string to Blob
        .method public instance void SetString(native int byteIndex, string 'value') cil managed
        {
            
            .locals init
            (
                int32   size
            )

            ldarga      'value'
            ldc.i4.0
            beq RET

            ldarg.2

            call		instance int32 [System.Runtime]System.String::get_Length()
            ldc.i4.1
            shl
            stloc.0
                                          
            ldarg.0     
            ldfld       native int DataTools.Memory.MemPtr::_ptr
            ldc.i4.0
            conv.i

            ceq
            brfalse CONT
            
            ldc.i4.0
            conv.i
            starg       byteIndex

            ldarg.0     
            
            
            ldloc.0
            ldc.i4.2
            add
            conv.i8

            call        instance bool DataTools.Memory.MemPtr::Alloc(int64)
            
            brfalse RET
    CONT:
            ldloc.0
            ldc.i4.0
            beq         NULLRET

            ldarg.0     
            ldfld       native int DataTools.Memory.MemPtr::_ptr
            ldarg.1     
            add

            ldarg.2
            ldc.i4.0
            ldelema     char

            sizeof      native int
            ldc.i4.4
            beq         INT32

            ldc.i4.4
            sub

INT32:
            ldloc.s     size
            conv.u4
                               
            cpblk
NULLRET:
        
            ldarg.0     
            ldfld       native int DataTools.Memory.MemPtr::_ptr
            ldarg.1     
            add
            ldloc.0
            add
            ldc.i4.0
            conv.u2
            stind.i2 
    RET:
            ret		
        }

        .method public instance string GrabUtf8String(native int byteIndex) cil managed 
        {
           .maxstack 8
            .locals init 
            (
                native int pinc, 
				native int orgbyte,
                int32  count, 
                uint8[] bytes
            )

			ldarg.s     byteIndex
			stloc.s     orgbyte

            ldarg.s     0
            ldind.i
            ldarg.s     byteIndex
            add
            
            starg.s     byteIndex
			ldarg.s     byteIndex
			stloc.s     pinc

    SCANLOOP:
            
            ldloc.s     pinc
            ldind.u1
            ldc.i4.0
            beq FINISHER
                
            ldloc.s     pinc
            ldc.i4.1
            add
            stloc.s     pinc
            br SCANLOOP

    FINISHER:		
           
            ldloc.s     pinc
            ldarg.s     byteIndex
            
            sub
           
            stloc.s     count
            ldloc.s     count
            ldc.i4.0
            ceq			
            
            brfalse		RET
            
            ldnull
            ret
        
        RET:
			ldarg.s     0
			ldloc.s     orgbyte
            ldloc.s     count
            call		instance uint8[] DataTools.Memory.MemPtr::GrabBytes(native int, int32)
            
            stloc.s     bytes
            call		class [System.Runtime]System.Text.Encoding [System.Runtime]System.Text.Encoding::get_UTF8()
            
            ldloc.s     bytes
            callvirt	instance string [System.Runtime]System.Text.Encoding::GetString(uint8[])	
                    
            ret        
		}

        // Get a null-terminated Unicode string from a pointer.
        .method public instance string GrabString(native int byteIndex) cil managed 
        {
            .maxstack 3
            .locals init 
            (
                native int strPtr, 
                int16 pinned ch,
                int32 count,
                string stout 
            )
    
            ldarg.s 0
            ldind.i

            stloc.s     strPtr
            ldloc.s     strPtr

            ldc.i4.0
            conv.i
            beq         RET

            ldloc.s     strPtr
            ldarg.s     byteIndex
            add
        	
			stloc.s     strPtr
            ldloc.s     strPtr
            starg.s     byteIndex

    SCANLOOP:

            ldloc.s     strPtr
            ldind.u2
            ldc.i4.0

            beq			FINISHER

            ldloc.s     strPtr
            ldc.i4.2
            add
            stloc.s     strPtr

            br			SCANLOOP

    FINISHER:

            ldloc.s     strPtr
            ldarg.s     byteIndex
            sub
            conv.i4
            stloc.s     count
            ldloc.s     count
            ldc.i4.0
            beq			RET

            ldc.i4.0
    
            ldloc.s     count
            ldc.i4.1
            shr
            
            newobj		instance void [System.Runtime]System.String::.ctor(char, int32)
            
            stloc.s     stout
            ldloc.s     stout
            ldc.i4.0
            ldelema		char

            sizeof      native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            ldarg.s     byteIndex
            ldloc.s     count
                                
            cpblk 

    RET:
            ldloc.s     stout
            ret
        }



        // Get a Unicode string of absolute length from a pointer.
        .method public instance string GrabString(native int byteIndex, int32 length) cil managed 
        {
            .maxstack 3
            .locals init 
            (
                native int strPtr, 
                int16 pinned ch,
                int32 count,
                string stout 
            )

            ldarg.s     length
            ldc.i4.1
            shl
            starg.s     length
    
            ldarg.s 0
            ldind.i

            stloc.s     strPtr
            ldloc.s     strPtr

            ldc.i4.0
            conv.i
            beq         RET

            ldloc.s     strPtr
            ldarg.s     byteIndex
            add
        	
			stloc.s     strPtr
            ldloc.s     strPtr
            starg.s     byteIndex

            ldloc.s     strPtr
            ldarg.s     length
            conv.i
            add
            stloc.s     strPtr

            ldloc.s     strPtr
            ldarg.s     byteIndex
            sub
            conv.i4
            stloc.s     count
            ldloc.s     count
            ldc.i4.0
            beq			RET

            ldc.i4.0
    
            ldloc.s     count
            ldc.i4.1
            shr

            newobj		instance void [System.Runtime]System.String::.ctor(char, int32)

            stloc.s     stout
            ldloc.s     stout
            ldc.i4.0
            ldelema		char

            sizeof      native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            ldarg.s     byteIndex
            ldloc.s     count
                                
            cpblk 

    RET:
            ldloc.s     stout
            ret
        }




        /////////////////////////////////////////////////
        //                                             //
        //  Functions to grab/set strings indirect.    //
        //                                             //
        /////////////////////////////////////////////////
        
        // Set a null-terminated Unicode string into a pointer.
        .method public instance void SetStringAtPointer(native int byteIndex, string str) cil managed 
        {
           .maxstack 3
            .locals init 
            (
                native int pinc, 
                native int nuln,
                int32 count 
            )

			ldarg.s     str
            call		instance int32 [System.Runtime]System.String::get_Length()
            

            ldc.i4.1
            shl
            stloc       count

            ldarg       str
            ldc.i4.0
            ldelema		char

            sizeof		native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            stloc       pinc
                    
            ldarg.s 0
            ldind.i

            ldarg.s     byteIndex

            add
			
			ldind.i

            ldloc       pinc
            ldloc       count
            
                    
            cpblk 

    RET:
            ret
        }

		
        // Set a null-terminated Unicode string into a pointer.
        .method public instance void SetStringAtPointerIndex(native int index, string str) cil managed 
        {
           .maxstack 3
            .locals init 
            (
                native int strPtr, 
                native int nuln,
                int32 count 
            )
			
			ldarg.s     index
			sizeof      native int
			mul
			starg.s     index

			ldarga.s    str
            ldc.i4.0
            conv.i
            beq         RET

            ldarg.s     str
            callvirt	instance int32 [System.Runtime]System.String::get_Length()
            
            ldc.i4.1
            shl
            stloc.s     count

            ldarg.s     str
            ldc.i4.0
            ldelema		char

            sizeof		native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            stloc.s     strPtr

            ldarg.s 0
            ldind.i
        	
	        ldarg.s     index
            sizeof      native int
            mul

            add

            ldind.i
		
            ldloc.s     strPtr
            ldloc.s     count
                    
            cpblk 
    RET:
            ret
        }


		// Get a null-terminated Unicode string indirectly.

        .method public instance string GrabStringFromPointerAt(native int index) cil managed 
        {
            .maxstack 3
            .locals init 
            (
                native int strPtr, 
                int16 pinned ch,
                int32 count,
                string stout 
            )
    
			ldc.i4.0
			conv.i

            stloc.s     count
            
            ldarg.s 0
            ldind.i

            ldarg.s     index
			sizeof      native int
			mul

            add
			ldind.i
        	
			stloc.s     strPtr
			ldloc.s     strPtr
			ldc.i4.0
			conv.i
			beq RET

			ldloc.s     strPtr
			starg.s     index

    SCANLOOP:

            ldloc.s     strPtr
            ldind.u2
            ldc.i4.0

            beq			FINISHER

            ldloc.s     strPtr
            ldc.i4.2
            add
            stloc.s     strPtr

            br			SCANLOOP

    FINISHER:

            ldloc.s     strPtr
            ldarg.s     index
            sub
            conv.i4
            stloc.s     count
            ldloc.s     count
            ldc.i4.0
            beq			RET

            ldc.i4.0
    
            ldloc.s     count
            ldc.i4.1
            shr

            newobj		instance void [System.Runtime]System.String::.ctor(char, int32)

            stloc.s     stout
            ldloc.s     stout
            ldc.i4.0
            ldelema		char

            sizeof      native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            ldarg.s     index
            ldloc.s     count
                                
            cpblk 

    RET:
            ldloc.s     stout
            ret
        }


        .method public instance string GrabStringFromPointerAtAbsolute(native int byteIndex) cil managed 
        {
           .maxstack 3
            .locals init 
            (
                native int pinc, 
                int16 pinned ch,
                int32 count,
                string stout 
            )
    
			ldc.i4.0
			conv.i

            stloc.s     count


            ldarg.s 0
            ldind.i
            ldarg       byteIndex
            add
			ldind.i
        	
			stloc.s     pinc
			ldloc.s     pinc
			ldc.i4.0
			conv.i
			beq RET

			ldloc.s     pinc
			starg 1

    SCANLOOP:

            ldloc.s     pinc
            ldind.u2
            ldc.i4.0

            beq			FINISHER

            ldloc.s     pinc
            ldc.i4.2
            add
            stloc.s     pinc

            br			SCANLOOP

    FINISHER:

            ldloc.s     pinc
            ldarg.s     byteIndex
            sub
            conv.i4
            stloc.s     count
            ldloc.s     count
            ldc.i4.0
            beq			RET

            ldc.i4.0
    
            ldloc.s     count
            ldc.i4.1
            shr

            newobj		instance void [System.Runtime]System.String::.ctor(char, int32)
            

            stloc.s     stout
            ldloc.s     stout
            ldc.i4.0
            ldelema		char

            sizeof      native int
            ldc.i4.4
            beq			INT32

            // Special Case for 64-bit processors, subtract 4 from the pointer. I have no idea why it works.
    INT64:
            ldc.i4.4
            sub

    INT32:
            ldarg.s     byteIndex
            ldloc.s     count
            
                    
            cpblk 

    RET:
            ldloc.3
            ret
        }



    }



}