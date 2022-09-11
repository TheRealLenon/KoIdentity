// KoIdentity Copyright (C) 2022 Tekoding. All Rights Reserved.
// 
// Created: 2022.05.29
// 
// Authors: TheRealLenon
// 
// Licensed under the MIT License. See LICENSE.md in the project root for license
// information.
// 
// KoIdentity is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the MIT
// License for more details.

using System;
using FluentAssertions;
using NUnit.Framework;
using Tekoding.KoIdentity.Abstraction.Errors;

namespace Tekoding.KoIdentity.Abstraction.Test.Errors;

/// <summary>
/// Defines a set of unit tests testing the <see cref="OperationResult"/>.
/// </summary>
public class OperationResultTests
{
    /// <summary>
    /// Checking the <see cref="OperationResult"/> to throw an <see cref="InvalidOperationException"/> when no errors
    /// are defined on a failed operation.
    /// </summary>
    [Test]
    public void OperationResult_ValidateInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => { OperationResult.Failed(); });
    }
    
    /// <summary>
    /// Checking the <see cref="OperationResult"/> to return the assigned error on a failed operation.
    /// </summary>
    [Test]
    public void OperationResult_ValidateFailedOperationResult()
    {
        var objectNullFailureExampleError = ErrorDescriber.ObjectNullFailure();

        var operationResult = OperationResult.Failed(objectNullFailureExampleError);

        operationResult.State.Should().BeFalse();
        operationResult.Payload.Should().NotBeNull().And.BeOfType<Error[]>();
        operationResult.ErrorCount.Should().Be(1);
        operationResult.Payload.As<Error[]>().Should().Contain(objectNullFailureExampleError);
        operationResult.ToString().Should().ContainAll(objectNullFailureExampleError.Code);

    }

    /// <summary>
    /// Checking the <see cref="OperationResult"/> to return no payload on a succeeded operation
    /// </summary>
    [Test]
    public void OperationResult_ValidateSuccessfulOperationResultWithoutPayload()
    {
        var operationResult = OperationResult.Success;

        operationResult.State.Should().BeTrue();
        operationResult.Payload.Should().BeNull();
        operationResult.ErrorCount.Should().Be(0);
        operationResult.ToString().Should().Contain("Succeeded");
    }

    /// <summary>
    /// Checking the <see cref="OperationResult"/> to contain a payload on a succeeded operation when desired.
    /// </summary>
    [Test]
    public void OperationResult_ValidateSuccessfulOperationResultWithPayload()
    {
        var examplePayload = Guid.NewGuid();
        var operationResult = OperationResult.SuccessWithPayload(examplePayload);
        
        operationResult.State.Should().BeTrue();
        operationResult.Payload.Should().NotBeNull().And.BeOfType<Guid>().And.Be(examplePayload);
        operationResult.ErrorCount.Should().Be(0);
        operationResult.ToString().Should().Contain("Succeeded");
    }
}